using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SearchableControls
{
    /// <summary>
    /// An extension of the Framework's TreeView control that allows the user to search for text in the control
    /// by pressing CTRL-F or using the context menu.
    /// </summary>
    /// <remarks>
    /// <para>To use, simply build the SearchableControls library and add a reference it in your project. The
    /// SearchableTreeView control should appear in the SearchableControls tab of the Visual Studio toolbox.
    /// Drag this control to your forms in the way you would a standard TreeView.</para>
    /// 
    /// <para>You may wish to give your forms an Edit/Find menu item with a specified shortcut of Ctrl-F. 
    /// This should call the OpenFindDialog() function of the main searchable control, or in the case of
    /// multiple searchable controls, the focused control. You could provide the same option from toolbars.</para>
    /// 
    /// <para>By default this form will just search the Text field of each TreeViewNode. A mechanism exists to
    /// provide a delegate to perform an alternative searching procedure.</para>
    /// 
    /// <para>As you can see the class is derived directly from TreeView so can do everything that the standard
    /// TreeView can do.</para>
    ///</remarks>
    public partial class SearchableTreeView : TreeView, ISearchable
    {
        /// <summary>
        /// Delegate of node searching function.
        /// </summary>
        /// <param name="node">The TreeNode to search</param>
        /// <param name="regularExpression">The regular expression to use to match text</param>
        /// <returns>'True' if the treeNode is deemed to have matched</returns>
        public delegate bool NodeSearchDelegate(TreeNode node, Regex regularExpression);

        /// <summary>
        /// Node searching function
        /// </summary>
        /// <remarks>
        /// This is set to a search of the Text property of the treeNode, but can be overridden by the
        /// client to provide custom search facilities of whatever the node conceptually contains, typically
        /// by using the node's Tag value to link it to an object.
        /// </remarks>
        [DesignerSerializationVisibility(0)]
        public NodeSearchDelegate NodeSearcher
        {
            get { return nodeSearcher; }
            set { nodeSearcher = value; }
        }

        private NodeSearchDelegate nodeSearcher;

        /// <summary>
        /// Construct a SearchableTreeView treeview control
        /// </summary>
        public SearchableTreeView()
        {
            InitializeComponent();

            nodeSearcher = new NodeSearchDelegate(DefaultNodeSearch); // create default search delegate

            // Currently there is no designer support for adding menu item event handlers
            findToolStripMenuItem.Click += new EventHandler(findToolStripMenuItem_Click);
        }
        
        /// <summary>
        /// Find Text from context menu
        /// </summary>
        void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            findDialog1.Show();
        }
        
        /// <summary>
        /// Handle key events. Used to provide find functions
        /// </summary>
        /// <param name="sender">Standard system parameter</param>
        /// <param name="e">Standard system parameter</param>
        protected void SearchableTreeView_KeyDown(object sender, KeyEventArgs e)
        {
            // Control F pressed, for 'Find'?
            if (e.KeyCode == Keys.F && e.Modifiers == Keys.Control)
            {
                findDialog1.Show();
                e.SuppressKeyPress = true; // don't pass the event down
            }
            // F3 pressed, for search again?
            else if (e.KeyCode == Keys.F3 && e.Modifiers == Keys.None)
            {
                findDialog1.FindNext();
                e.SuppressKeyPress = true; // don't pass the event down
            }

        }
        
        /// <summary>
        /// Default function to search a node
        /// </summary>
        /// <param name="treeNode">The TreeNode to search</param>
        /// <param name="regularExpression">The regular expression to use to match text</param>
        /// <returns>'True' if the treeNode is deemed to have matched</returns>
        private bool DefaultNodeSearch(TreeNode treeNode, Regex regularExpression)
        {
            return regularExpression.IsMatch(treeNode.Text);
        }

        /// <summary>
        /// The various modes that the recursive scan can be in
        /// </summary>
        private enum TreeSearchState
        {
            NotStarted,
            Started,
            MatchMade,
            HitEndNode
        }

        /// <summary>
        /// A recursive 'sub search' command is used to search part of the tree
        /// </summary>
        /// <param name="regularExpression">The regular expression to use to match text</param>
        /// <param name="treeNodeCollection">The collection of nodes to search down from</param>
        /// <param name="startAfterNode">The node that searching actually begins from
        /// (otherwise just walk the tree until this node is found)</param>
        /// <param name="stopAtNode">A node to terminate the search at</param>
        /// <param name="state">Sends and returns the state of the recursive search</param>
        private void SubSearch(Regex regularExpression, TreeNodeCollection treeNodeCollection, TreeNode startAfterNode, TreeNode stopAtNode, ref SearchableTreeView.TreeSearchState state)
        {
            foreach (TreeNode treeNode in treeNodeCollection)
            {
                if (state == SearchableTreeView.TreeSearchState.Started) // Has the search started?
                {
                    if (treeNode == stopAtNode)
                    {
                        state = SearchableTreeView.TreeSearchState.HitEndNode;
                        return;
                    }


                    if (nodeSearcher(treeNode, regularExpression))
                    {
                        // We need to show search results even when the FindDialog is active
                        // This means turning off HideSelection if it is set.
                        // This unfortunately causes a slight flicker. One way to avoid this is to turn off HideSelection
                        // in individual control instances.
                        if (HideSelection)
                        {
                            // Ensure that the property is restored when the FindDialog is deactivated
                            findDialog1.Deactivate += new EventHandler(RestoreHideSelection);
                            HideSelection = false;
                        }
                        SelectedNode = treeNode;
                        SelectedNode.EnsureVisible();  // Make sure the result node is visible
                        state = SearchableTreeView.TreeSearchState.MatchMade;
                        return;
                    }
                }
                if (startAfterNode == treeNode)
                {
                    state = SearchableTreeView.TreeSearchState.Started;
                }

                // sub search child nodes
                SubSearch(regularExpression, treeNode.Nodes, startAfterNode, stopAtNode, ref state);
                if (state == SearchableTreeView.TreeSearchState.HitEndNode)
                {
                    return;
                }
                if (state == SearchableTreeView.TreeSearchState.MatchMade)
                {
                    return;
                }
            }
        }

        /// <summary>
        /// A record of the first node of a search series
        /// </summary>
        private TreeNode originalSelectionStart;

        private void findDialog1_SearchRequested(object sender, SearchEventArgs e)
        {
            if (e.FirstSearch)
            {
                // Store the selection start position on the first search so that when all searches are complete
                // this fact can be reported to the user in the find dialog.
                originalSelectionStart = SelectedNode;
            }

            // First part of search is between item after current selection (inclusive) and the end of the
            // document (exclusive), or the original search position position (exclusive) if this is greater
            // than the current selection position
            TreeNode searchFromBelow = SelectedNode;
            SearchableTreeView.TreeSearchState treeSearchState = SearchableTreeView.TreeSearchState.NotStarted;

            // A SubSearch function is used to search part of the tree
            SubSearch(e.SearchRegularExpression, Nodes, searchFromBelow, originalSelectionStart, ref treeSearchState);
            if (treeSearchState == SearchableTreeView.TreeSearchState.MatchMade)
            {
                e.Successful = true;// We have a match
            }
            else if (treeSearchState != SearchableTreeView.TreeSearchState.HitEndNode)
            {
                // No match? We hit end of document
                // Retry from the beginning if the original start position is before or equal to the current selection
                e.RestartedFromDocumentTop = true; // The user is informed that the search started from the top
                treeSearchState = SearchableTreeView.TreeSearchState.Started;

                // Search first half of the document
                SubSearch(e.SearchRegularExpression, Nodes, null, originalSelectionStart, ref treeSearchState);
                if (treeSearchState == SearchableTreeView.TreeSearchState.MatchMade)
                {
                    e.Successful = true; // We have a match
                }
            }

        }
        
        /// <summary>
        /// Put this control's HideSelection property back to normal when the FindDialog is deactivated
        /// </summary>
        /// <remarks>
        /// This unfortunately causes a slight flicker. One way to avoid this is to turn off HideSelection
        /// in individual control instances.
        /// </remarks>
        void RestoreHideSelection(object sender, EventArgs e)
        {
            HideSelection = true;
        }
        
        #region ISearchable Members

        /// <summary>
        /// Return the FindDialog associated with the control
        /// </summary>
        public FindDialog FindDialog
        {
            get
            {
                return findDialog1;
            }
        }

        #endregion
    }
}
