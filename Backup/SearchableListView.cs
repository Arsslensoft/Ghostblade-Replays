using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace SearchableControls
{
    /// <summary>
    /// An extension of the Framework's ListView control that allows the user to search for text in the tree
    /// by pressing CTRL-F, or using the context menu. It also includes "select all" functionality.
    /// </summary>
    /// <remarks>
    /// <para>Part of SearchableControls written by Jim Blackler (jimblackler@gmail.com), August 2006</para>
    /// 
    /// <para>To use, simply build the SearchableControls library and add a reference it in your project. The
    /// SearchableListView control should appear in the SearchableControls tab of the Visual Studio toolbox.
    /// Drag this control to your forms in the way you would a standard ListView.</para>
    /// 
    /// <para>You may wish to give your forms an Edit/Find menu item with a specified shortcut of Ctrl-F. 
    /// This should call the OpenFindDialog() function of the main searchable control, or in the case of
    /// multiple searchable controls, the focused control. You could provide the same option from toolbars.</para>
    /// 
    /// <para>By default this form will just search the Text field of each ListViewNode. A mechanism exists to
    /// provide a delegate to perform an alternative searching procedure.</para>
    /// 
    /// <para>As you can see the class is derived directly from ListView so can do everything that the standard
    /// ListView can do.</para>
    ///</remarks>
    public partial class SearchableListView : ListView, ISearchable
    {

        /// <summary>
        /// Delegate of node searching function.
        /// </summary>
        /// <param name="node">The TreeNode to search</param>
        /// <param name="regularExpression">The regular expression to use to match text</param>
        /// <returns>'True' if the treeNode is deemed to have matched</returns>
        public delegate bool NodeSearchDelegate(ListViewItem node, Regex regularExpression);

        /// <summary>
        /// Node searching function
        /// </summary>
        /// <remarks>
        /// This is set to a search of the Text property of the listViewItem, but can be overridden by the
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
        /// Construct a SearchableListView treeview control
        /// </summary>
        public SearchableListView()
        {
            InitializeComponent();

            nodeSearcher = new NodeSearchDelegate(DefaultNodeSearch); // create default search delegate

            // Currently there is no designer support for adding menu item event handlers
            findToolStripMenuItem.Click += new EventHandler(findToolStripMenuItem_Click);
            selectAllToolStripMenuItem.Click += new EventHandler(selectAllToolStripMenuItem_Click);
        }

        /// <summary>
        /// Handle key events. Used to provide find functions
        /// </summary>
        /// <param name="sender">Standard system parameter</param>
        /// <param name="e">Standard system parameter</param>
        protected void SearchableListView_KeyDown(object sender, KeyEventArgs e)
        {
            // Control A pressed, for 'Select All'?
            if (e.KeyCode == Keys.A && e.Modifiers == Keys.Control)
            {
                SelectAll();
                e.SuppressKeyPress = true; // don't pass the event down
            }
            // Control F pressed, for 'Find'?
            else if (e.KeyCode == Keys.F && e.Modifiers == Keys.Control)
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
        /// Select all the list items
        /// </summary>
        /// <remakrs>
        /// Not supplied in the basic Framework version
        /// </remakrs>
        public void SelectAll()
        {
            // Select every item
            foreach (ListViewItem item in Items)
            {
                item.Selected = true;
            }
        }

        /// <summary>
        /// Default function to search a node
        /// </summary>
        /// <param name="listViewItem">The ListViewItem to search</param>
        /// <param name="regularExpression">The regular expression to use to match text</param>
        /// <returns>'True' if the treeNode is deemed to have matched</returns>
        private bool DefaultNodeSearch(ListViewItem listViewItem, Regex regularExpression)
        {
            // Search all of the sub items
            foreach (ListViewItem.ListViewSubItem subItem in listViewItem.SubItems)
            {
                if (regularExpression.IsMatch(subItem.Text))
                    return true;
            }
            
            // Search the main item text
            return regularExpression.IsMatch(listViewItem.Text);
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

        /// <summary>
        /// Context menu is being opened. Grey out appropriate selections
        /// </summary>
        private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            // Select All only available if there are unselected items
            bool anyUnselected = false;
            foreach (ListViewItem item in Items)
            {
                if (!item.Selected)
                {
                    anyUnselected = true;
                    break;
                }
            }
            selectAllToolStripMenuItem.Enabled = anyUnselected;
        }

        /// <summary>
        /// Find Text from context menu
        /// </summary>
        void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            findDialog1.Show();
        }

        /// <summary>
        /// Select All from context menu
        /// </summary>
        void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectAll();
        }

           
        /// <summary>
        /// Search a subset of the list view's items', by index range
        /// </summary>
        /// <param name="regularExpression">The regular expression to use to match text</param>
        /// <param name="start">The list index to start searching from</param>
        /// <param name="end">The list index after the one to stop searching at</param>
        /// <returns>'True' if the search was successful</returns>
        protected bool SubSearch(Regex regularExpression, int start, int end)
        {
            for (int idx = start; idx < end; idx++)
            {
                if (this.nodeSearcher(Items[idx], regularExpression))
                {
                    // We need to show search results even when the FindDialog is active
                    // This means turning off HideSelection if it is set.
                    // This unfortunately causes a slight flicker. One way to avoid this is to turn off HideSelection
                    // in individual control instances.
                    if (HideSelection)
                    {
                        this.findDialog1.Deactivate += new EventHandler(this.RestoreHideSelection);
                        HideSelection = false;
                    }
                    SelectedIndices.Clear();
                    Items[idx].Selected = true;
                    EnsureVisible(idx);
                    return true; //found a match
                }
            }
            return false;
        }

        /// <summary>
        /// A record of the first node of a search series
        /// </summary>
        protected int originalSelectionStart;

        /// <summary>
        /// Perform the search on the list view
        /// </summary>
        /// <param name="sender">Sending object</param>
        /// <param name="e">Parameters relating to the search event</param>
        private void findDialog1_SearchRequested(object sender, SearchEventArgs e)
        {
            int selectionStart;
            int endSearch;

            // Calculate the first node for the search
            if (SelectedIndices.Count > 0)
            {
                selectionStart = SelectedIndices[0];
            }
            else
            {
                // No selection - search from the top of the document
                selectionStart = 0;
            }

            // Store the selection start position on the first search so that when all searches are complete
            // this fact can be reported to the user in the find dialog.
            if (e.FirstSearch)
            {
                originalSelectionStart = selectionStart;
            }
            
            // Calculate the end point
            if (originalSelectionStart > selectionStart)
            {
                // Final node is before end of document - just search to there
                endSearch = this.originalSelectionStart;
            }
            else
            {
                endSearch = Items.Count;
            }

            // Search the first subsection - from current selection position to the end of the document,
            // or the original starting point.
            bool match = this.SubSearch(e.SearchRegularExpression, selectionStart + 1, endSearch);
            if (!match && (this.originalSelectionStart <= selectionStart))
            {
                // No match .. search the first half of the document
                match = this.SubSearch(e.SearchRegularExpression, 0, this.originalSelectionStart);
                if (match)
                {
                    // We may wish to tell the user we have started from the top of the document
                    e.RestartedFromDocumentTop = true;
                }
            }

            if (match)
            {
                // A match was found - tell the user
                e.Successful = true;
            }

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
