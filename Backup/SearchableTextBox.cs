using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SearchableControls
{
    /// <summary>
    /// An extension of the Framework's TextBox control that allows the user to search for text in the
    /// control by pressing CTRL-F or by using the context menu.
    /// It also returns the standard CTRL-A shortcut to select all the text in the control.
    /// </summary>
    /// <remarks>
    /// <para>Part of SearchableControls written by Jim Blackler (jimblackler@gmail.com), August 2006</para>
    /// 
    /// <para>To use, simply build the SearchableControls library and add a reference it in your project. The
    /// SearchableTextBox control should appear in the SearchableControls tab of the Visual Studio
    /// toolbox. Drag this control to your forms in the way you would a standard TextBox.</para>
    /// 
    /// <para>You may wish to give your forms an Edit/Find menu item with a specified shortcut of Ctrl-F. 
    /// This should call the OpenFindDialog() function of the main searchable control, or in the case of
    /// multiple searchable controls, the focused control. You could provide the same option from toolbars.</para>
    /// 
    /// <para>As you can see the class is derived directly from TextBox so can do everything that the standard
    /// TextBox can do.</para>
    ///</remarks>
    public partial class SearchableTextBox : TextBox, ISearchable
    {
        /// <summary>
        /// Construct a SearchableTextBox textbox
        /// </summary>
        public SearchableTextBox()
        {
            InitializeComponent();

            findDialog1.ReplaceAvailable = !ReadOnly; // Find is not offered if the control is read-only

            // Currently there is no designer support for adding menu item event handlers
            undoToolStripMenuItem.Click += new EventHandler(undoToolStripMenuItem_Click);
            cutToolStripMenuItem.Click += new EventHandler(cutToolStripMenuItem_Click);
            copyToolStripMenuItem.Click += new EventHandler(copyToolStripMenuItem_Click);
            pasteToolStripMenuItem.Click += new EventHandler(pasteToolStripMenuItem_Click);
            deleteToolStripMenuItem.Click += new EventHandler(deleteToolStripMenuItem_Click);
            selectAllToolStripMenuItem.Click += new EventHandler(selectAllToolStripMenuItem_Click);
            findToolStripMenuItem.Click += new EventHandler(findToolStripMenuItem_Click);
            replaceToolStripMenuItem.Click += new EventHandler(replaceToolStripMenuItem_Click);

        }

        /// <summary>
        /// Handle key events
        /// </summary>
        /// <remarks>
        /// Used to process custom shortcuts
        /// </remarks>
        /// <param name="sender">Standard system parameter</param>
        /// <param name="e">Standard system parameter</param>
        protected void SearchableTextBox_KeyDown(object sender, KeyEventArgs e)
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
                NewSearch(false);
                e.SuppressKeyPress = true; // don't pass the event down
            }
            else if (e.KeyCode == Keys.H && e.Modifiers == Keys.Control)
            {
                if (!ReadOnly) // Ctrl-H only available in for non read-only controls
                {
                    NewSearch(true);
                    e.SuppressKeyPress = true;
                }
            }
            // F3 pressed, for search again?
            else if (e.KeyCode == Keys.F3 && e.Modifiers == Keys.None)
            {
                findDialog1.FindNext();
                e.SuppressKeyPress = true; // don't pass the event down
            }
            // First press of Escape removes the search dialog if it's present
            else if (e.KeyCode == Keys.Escape && e.Modifiers == Keys.None)
            {
                if (findDialog1.Visible)
                {
                    findDialog1.Close();
                    e.SuppressKeyPress = true;
                }
            }
            else if (e.KeyCode == Keys.Z && e.Modifiers == Keys.Control)
            {
                // Undo - intercepted to give the control a chance to undo replace operations
                Undo(); // Local version called
                e.SuppressKeyPress = true; // don't pass the event down
            }
            else if (e.KeyCode == Keys.Y && e.Modifiers == Keys.Control)
            {
                // Redo - intercepted to give the control a chance to redo replace operations
                Redo(); // Local version called
                e.SuppressKeyPress = true; // don't pass the event down
            }
        }

        /// <summary>
        /// Helper function for selecting a word
        /// </summary>
        /// <param name="p">A character from the possible word</param>
        /// <returns>True if this is a word character</returns>
        static private bool IsWordChar(char p)
        {
            if (p >= 'a' && p <= 'z')
                return true;
            if (p >= 'A' && p <= 'Z')
                return true;
            if (p >= '0' && p <= '9')
                return true;
            if (p == '_')
                return true;

            return false;
        }

        /// <summary>
        /// A record of the control's text before any replace operations
        /// </summary>
        string textBeforeReplace;

        /// <summary>
        /// A record of the control's text after most recent replace operations
        /// </summary>
        string textAfterReplace;

        /// <summary>
        /// Overriden Undo to undo replace operations
        /// </summary>
        /// <remarks>
        /// Would be a lot simpler if the framework controls gave you access to their undo stack
        /// </remarks>
        public new void Undo()
        {
            if (Text.Equals(textAfterReplace))
            {
                Text = textBeforeReplace;
            }
            else
            {
                // Call ordinary undo
                base.Undo();
            }
        }

        /// <summary>
        /// Overridden CanUndo to include local undo operation
        /// </summary>
        public new bool CanUndo
        {
            get
            {
                if (Text.Equals(textAfterReplace))
                {
                    return true; // Next undo would be our replace cancel operation
                }
                else
                {
                    // Call ordinary CanUndo
                    return base.CanUndo;
                }
            }
        }

        /// <summary>
        /// Redo to redo replace operations
        /// </summary>
        public void Redo()
        {
            if (Text.Equals(textBeforeReplace))
            {
                Text = textAfterReplace;
            }

        }

        /// <summary>
        /// Start a new search
        /// </summary>
        private void NewSearch(bool replaceMode)
        {
            if (findDialog1.ReplaceAvailable)
            {
                textBeforeReplace = Text;
            }

            // If no text is selected, select the word
            if (SelectedText.Length == 0)
            {
                int wordStart = SelectionStart;
                while (wordStart > 0 && IsWordChar(Text[wordStart - 1]))
                {
                    wordStart--;
                }
                int wordEnd = SelectionStart;
                while (wordEnd < Text.Length && IsWordChar(Text[wordEnd]))
                {
                    wordEnd++;
                }

                // Store the selection start position on the first search so that when all searches are complete
                // this fact can be reported to the user in the find dialog.
                originalSelectionStart = wordEnd; // matching behaviour of various MS apps

                findDialog1.Show(Text.Substring(wordStart, wordEnd - wordStart), replaceMode);
            }
            else if (!SelectedText.Contains("\n")) // if selection is single line, use that as default
            {
                originalSelectionStart = SelectionStart; // matching behaviour of various MS apps
                findDialog1.Show(SelectedText, replaceMode);
            }
            else
            {
                originalSelectionStart = SelectionStart;
                findDialog1.Show(replaceMode);
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

        /// <summary>
        /// Enable the context menu items that can be invoked
        /// </summary>
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            undoToolStripMenuItem.Enabled = CanUndo;
            cutToolStripMenuItem.Enabled = (!ReadOnly && SelectionLength > 0);
            copyToolStripMenuItem.Enabled = (SelectionLength > 0);
            pasteToolStripMenuItem.Enabled = !ReadOnly;
            deleteToolStripMenuItem.Enabled = (!ReadOnly && SelectionLength > 0);
            selectAllToolStripMenuItem.Enabled = (SelectionLength < Text.Length);
            replaceToolStripMenuItem.Enabled = findDialog1.ReplaceAvailable;
        }

        /// <summary>
        /// Undo from context menu
        /// </summary>
        void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Undo();
        }

        /// <summary>
        /// Cut from context menu
        /// </summary>
        void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cut();
        }

        /// <summary>
        /// Copy from context menu
        /// </summary>
        void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Copy();
        }

        /// <summary>
        /// Paste from context menu
        /// </summary>
        void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Paste();
        }

        /// <summary>
        /// Delete from context menu
        /// </summary>
        void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SendKeys.Send("{DELETE}"); // Send the delete key
        }

        /// <summary>
        /// Select All from context menu
        /// </summary>
        void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectAll();
        }

        /// <summary>
        /// Find Text from context menu
        /// </summary>
        void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewSearch(false);
        }

        /// <summary>
        /// Replace from context menu
        /// </summary>
        void replaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewSearch(true);
        }

        /// <summary>
        /// Search a sub-portion of the text
        /// </summary>
        internal bool SubSearch(Regex regularExpression, int start, int end)
        {
            Match match = regularExpression.Match(Text, start, end - start);
            if (match.Success)
            {
                // We need to show search results even when the FindDialog is active
                // This means turning off HideSelection if it is set.
                // This unfortunately causes a slight flicker. One way to avoid this is to turn off HideSelection
                // in individual control instances.
                if (HideSelection)
                {
                    // ensure that the property is restored when the FindDialog is deactivated
                    findDialog1.Deactivate += new EventHandler(RestoreHideSelection);
                    HideSelection = false;
                }

                Select(match.Index, match.Length);

                try
                {
                    ScrollToCaret(); // try/caught because this has been known to fail unexpectedly
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Stores the selection start position on the first search so that when all searches are complete
        /// this fact can be reported to the user in the find dialog.
        /// </summary>
        protected int originalSelectionStart;

        /// <summary>
        /// Perform the search on the text box
        /// </summary>
        /// <param name="sender">Sending object</param>
        /// <param name="e">Parameters relating to the search event</param>
        protected void findDialog1_SearchRequested(object sender, SearchEventArgs e)
        {
            int startSearch;
            int endSearch;

            if (e.FirstSearch)
            {
                startSearch = originalSelectionStart;
                endSearch = Text.Length;
            }
            else
            {
                // First part of search is between character after current selection (inclusive) and the end of the
                // document (exclusive), or the original search position position (exclusive) if this is greater
                // than the current selection position
                startSearch = SelectionStart + SelectionLength;

                if (originalSelectionStart >= startSearch)
                {
                    endSearch = originalSelectionStart;
                }
                else
                {
                    endSearch = Text.Length;
                }
            }

            bool match;

            match = SubSearch(e.SearchRegularExpression, startSearch, endSearch);


            if (!match && endSearch == Text.Length) // no match? retry from the beginning if the original start position is before or equal to the current search
            {
                // second search is from the start of the document to the original search position (exclusive)

                match = SubSearch(e.SearchRegularExpression, 0, originalSelectionStart);

                if (match)
                {
                    e.RestartedFromDocumentTop = true; // The user is informed that the search started from the top
                }
            }

            if (match)
            {
                e.Successful = true;
            }
        }

        /// <summary>
        /// The find dialog has requested a replace operation on the most recently selected text
        /// </summary>
        private void findDialog1_ReplaceRequested(object sender, ReplaceEventArgs e)
        {
            // Unfortunately it does not seem possible to register this in the TextBox's undo stack
            SelectedText = e.ReplaceText;

            if (findDialog1.ReplaceAvailable)
            {
                textAfterReplace = Text;
            }
        }

        /// <summary>
        /// The read-only property has been changed on the control
        /// </summary>
        private void SearchableTextBox_ReadOnlyChanged(object sender, EventArgs e)
        {
            findDialog1.ReplaceAvailable = !ReadOnly;
        }

        #region ISearchable Members

        /// <summary>
        /// Return the FindDialog (if any) currently associated with this control
        /// </summary>
        public FindDialog FindDialog
        {
            get { return findDialog1; }
        }

        #endregion

        /// <summary>
        /// User pressed cancel button on find form
        /// </summary>
        private void findDialog1_CancelReplaceRequested(object sender, EventArgs e)
        {
            // Ensure we are overwriting what we expect
            if (Text.Equals(textAfterReplace))
            {
                Text = textBeforeReplace;
            }
        }
    }
}
