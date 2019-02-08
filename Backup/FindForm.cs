using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SearchableControls
{
    /// <summary>
    /// A dialog to find a user-supplied text string
    /// </summary>
    /// <remarks>
    /// <para>Part of SearchableControls written by Jim Blackler (jimblackler@gmail.com), August 2006</para>
    /// <para>A partner class to FindDialog which is the control applied to add search functionality
    /// to other controls.</para>
    /// </remarks>
    internal partial class FindForm : Form
    {
        /// <summary>
        /// Record of the FindDialog
        /// </summary>
        protected FindDialog findDialog;

        /// <summary>
        /// Make a simple find dialog
        /// </summary>
        /// <param name="_findDialog">The FindDialog parent object</param>
        /// <param name="defaultText">Default text for the input box</param>
        /// <param name="restoreData">Restore data (if any) to restore the user-changeable form properties from</param>
        /// <param name="formatter">Formatter to read the above</param>
        /// <param name="replaceMode">Start in replace mode?</param>
        /// <param name="offerReplace">Offer the user the chance to switch to Replace mode?</param>
        public FindForm(FindDialog _findDialog, Stream restoreData, IFormatter formatter, string defaultText, bool replaceMode, bool offerReplace)
        {
            findDialog = _findDialog;

            Owner = findDialog.ParentControl.FindForm();
            InitializeComponent();

            findDialog.SearchMode = FindDialog.SearchModes.Ready;

            
            replaceModeCheckBox.Visible = offerReplace;

            // Populate searchTypeComboBox
            foreach (FindForm.SearchType searchType in Enum.GetValues(typeof(FindForm.SearchType)))
            {
                // convert StringsThatLookLikeThis to Strings that look like this
                searchTypeComboBox.Items.Add(searchType.ToString()[0] + Regex.Replace(searchType.ToString().Substring(1), @"(\B[A-Z])", " $1").ToLower());
            }

            searchTypeComboBox.SelectedIndex = 0;
            if (restoreData == null)
            {
                replaceModeCheckBox.Checked = replaceMode; // apply this attribute here because it can change the size of the form

                // Generate an appropriate position for the dialog - just off the parent control, but on the screen
                Control parentControl = findDialog.ParentControl;

                // Get the current bounds of the screen
                Rectangle screenBounds = Screen.GetWorkingArea(this);

                // Try a position top right of the parent 
                Rectangle bounds = new Rectangle(parentControl.PointToScreen(new Point(parentControl.Width - Width, -Height)), Size);

                Rectangle intersection = Rectangle.Intersect(screenBounds, bounds);
                if (!intersection.Equals(bounds))
                {
                    // If intersection betwen screen bounds and dialog bounds is not exactly the same as the dialog bounds,
                    // some or all of the dialog is off screen.
                    // So, set the position to just below the bottom right of the parent control
                    bounds = new Rectangle(parentControl.PointToScreen(new Point(parentControl.Width - Width, parentControl.Height)), Size);
                }

                Bounds = bounds; // Appy the chosen bounds
            }
            else
            {
                // Restore the important settings from the previously recorded RestoreData
                ApplyRestoreData(restoreData, formatter);
                replaceModeCheckBox.Checked = replaceMode; // apply this attribute here because it can change the size of the form
            }

            ApplySearchMode(); // Set the initial visual setting

            if (defaultText != null)
            {
                searchHistoryComboBox.Text = defaultText;
            }

            Reshow();
        }

        /// <summary>
        /// Search button has been pressed
        /// </summary>
        private void searchButton_Click(object sender, EventArgs e)
        {
            // Add text to history
            if ((searchHistoryComboBox.Items.Count == 0) || !searchHistoryComboBox.Text.Equals(searchHistoryComboBox.Items[searchHistoryComboBox.Items.Count - 1]))
            {
                searchHistoryComboBox.Items.Add(searchHistoryComboBox.Text);
            }

            SearchPressed();
        }

        /// <summary>
        /// Replace button has been pressed
        /// </summary>
        private void replaceButton_Click(object sender, EventArgs e)
        {
            // Add text to history
            if ((replaceHistoryComboBox.Items.Count == 0) || !replaceHistoryComboBox.Text.Equals(replaceHistoryComboBox.Items[replaceHistoryComboBox.Items.Count - 1]))
            {
                replaceHistoryComboBox.Items.Add(replaceHistoryComboBox.Text);
            }

            // Action to take depends on the current search mode
            switch (findDialog.SearchMode)
            {
                case FindDialog.SearchModes.SearchFailed:
                case FindDialog.SearchModes.Ready:
                    // If no search in progress, this is equivalent to just pressing Search
                    SearchPressed();
                    break;
                case FindDialog.SearchModes.SearchAgain:
                case FindDialog.SearchModes.SearchFinished:
                    findDialog.Replace(replaceHistoryComboBox.Text); // replace
                    cancelButton.Enabled = true; // allow cancelling now
                    SearchPressed(); // then search
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Local record of number of matches done in 'find all' mode
        /// </summary>
        private int numberFindAllReplaces = 0;

        /// <summary>
        /// User pressed 'Replace All;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void replaceAllButton_Click(object sender, EventArgs e)
        {
            if (findDialog.SearchMode == FindDialog.SearchModes.Ready)
            {
                SearchPressed();
            }

            while (findDialog.SearchMode == FindDialog.SearchModes.SearchAgain)
            {
                findDialog.Replace(replaceHistoryComboBox.Text); // replace
                numberFindAllReplaces++;
                findDialog.Search();
            }

            if (findDialog.SearchMode == FindDialog.SearchModes.SearchFinished)
            {
                findDialog.Replace(replaceHistoryComboBox.Text); // replace last find
            }

            if (numberFindAllReplaces > 0)
            {
                new InfoForm(numberFindAllReplaces.ToString() + " occurance(s) replaced", Owner).ShowDialog();
                numberFindAllReplaces = 0;
            }

            cancelButton.Enabled = true;
        }

        /// <summary>
        /// Cancel button clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            findDialog.CancelReplace();
            cancelButton.Enabled = false; // cancelled now - can't do it again
        }

        /// <summary>
        /// User is typing in the text string box... make sure label doesn't say 'Find Again' any more
        /// </summary>
        private void searchHistoryComboBox_TextChanged(object sender, EventArgs e)
        {
            findDialog.SearchMode = FindDialog.SearchModes.Ready;

            // Grey out the search button if there is no text
            if (searchHistoryComboBox.Text.Length > 0)
            {
                searchButton.Enabled = true;
                replaceButton.Enabled = true;
                replaceAllButton.Enabled = true;
            }
            else
            {
                searchButton.Enabled = false;
                replaceButton.Enabled = false;
                replaceAllButton.Enabled = false;
            }
        }

        /// <summary>
        /// User changed search type... re-enable search button.
        /// </summary>
        private void searchTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            findDialog.SearchMode = FindDialog.SearchModes.Ready; // re-enable the search state
        }

        /// <summary>
        /// User checked ignore case checkbox... re-enable search button.
        /// </summary>
        private void ignoreCaseCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            findDialog.SearchMode = FindDialog.SearchModes.Ready; // re-enable the search state
        }

        /// <summary>
        /// User has pressed a key in the text box
        /// </summary>
        private void FindDialog_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.F) && (e.Modifiers == Keys.Control))
            {
                if (replaceModeCheckBox.Checked)
                {
                    // Switch to find mode
                    replaceModeCheckBox.Checked = false;
                }
                else
                {
                    searchHistoryComboBox.SelectAll(); // Prepare for new text entry
                    findDialog.SearchMode = FindDialog.SearchModes.Ready; // Re-enable search button
                }
                e.SuppressKeyPress = true;
            }
            if ((e.KeyCode == Keys.H) && (e.Modifiers == Keys.Control))
            {
                if (replaceModeCheckBox.Visible && !replaceModeCheckBox.Checked)
                {
                    // Switch to replace mode
                    replaceModeCheckBox.Checked = true;
                }
                else
                {
                    searchHistoryComboBox.SelectAll(); // Prepare for new text entry
                    findDialog.SearchMode = FindDialog.SearchModes.Ready; // Re-enable search button   
                }
                e.SuppressKeyPress = true;
            }
            if ((e.KeyCode == Keys.A) && (e.Modifiers == Keys.Control))
            {
                searchHistoryComboBox.SelectAll(); // Select all the text
                e.SuppressKeyPress = true; // Re-enable search button
            }
            else if ((e.KeyCode == Keys.F3) && (e.Modifiers == Keys.None))
            {
                SearchPressed(); // Activate the search
                e.SuppressKeyPress = true;  // Do not process key in parent
            }
            else if (e.KeyCode == Keys.Escape)
            {
                Close();
                e.SuppressKeyPress = true; // Do not process key in parent
            }
        }

        /// <summary>
        /// Invoke a search.
        /// </summary>
        protected void SearchPressed()
        {
            if (searchButton.Enabled) // only do if searching is enabled
            {
                string text;

                // Set up the regular expression to search with
                // Using regular expressions allows us massive power to customize search behavior
                // with out having to change the individuals' controls search code.
                RegexOptions options = RegexOptions.None;

                // Ignore case is simply a regular expression parameter
                if (ignoreCaseCheckBox.Checked)
                {
                    options |= RegexOptions.IgnoreCase;
                }

                switch ((SearchType)searchTypeComboBox.SelectedIndex)
                {
                    case SearchType.RegularExpression:
                        // If user selected Regular Expression, just pass the text directly
                        text = Regex.Escape(searchHistoryComboBox.Text).Replace(@"\*", ".*").Replace(@"\?", ".");
                        break;

                    case SearchType.Wildcards:
                        // Escapes the text, then converts wildcard tokens to regex equivalents
                        text = searchHistoryComboBox.Text;
                        break;

                    case SearchType.PlainTextSearch:
                    default:
                        // Just a plain text search... 'escape' the text so it can be used in the regular expression
                        text = Regex.Escape(searchHistoryComboBox.Text);
                        break;
                }
                Regex searchRegularExpression = null;
                try
                {
                    searchRegularExpression = new Regex(text, options);
                }
                catch (Exception exception)
                {
                    // relay the error to the user
                    MessageBox.Show(this, exception.Message, "Regular expression error");
                }

                if (searchRegularExpression != null)
                {
                    findDialog.SearchRegularExpression = searchRegularExpression;
                    findDialog.Search();
                    searchHistoryComboBox.SelectAll();
                }
            }

        }

        /// <summary>
        /// Regular expression forming the most recent search
        /// </summary>
        public Regex SearchRegularExpression
        {
            get { return searchRegularExpression; }
            set { searchRegularExpression = value; }
        }

        private Regex searchRegularExpression;

        /// <summary>
        /// Bring the dialog to the front and select all the text
        /// </summary>
        public void Reshow()
        {
            Show();
            BringToFront();
            searchHistoryComboBox.SelectAll(); // most Windows dialogs do this
            searchHistoryComboBox.Focus();
            findDialog.SearchMode = FindDialog.SearchModes.Ready;
        }

        /// <summary>
        /// Discovers if it is meaningful to attempt to FindNext
        /// </summary>
        /// <returns>'True' if it is meaningful to attempt to FindNext
        /// </returns>
        /// <remarks>
        /// Typically used by client applications to decide whether to 'grey out' menu or toolbar entries 
        /// relating to FindNext()
        /// </remarks>
        public bool FindNextIsAvailable()
        {
            return searchButton.Enabled;
        }

        /// <summary>
        /// Search types for the combo box
        /// </summary>
        private enum SearchType
        {
            PlainTextSearch,
            Wildcards,
            RegularExpression
        }

        /// <summary>
        /// Set the visual style of the current search mode
        /// </summary>
        internal void ApplySearchMode()
        {
            switch (findDialog.SearchMode)
            {
                case FindDialog.SearchModes.Ready:
                    searchButton.Text = "&Search";
                    if (searchHistoryComboBox.Text.Length > 0)
                    {
                        searchButton.Enabled = true;
                        return;
                    }
                    searchButton.Enabled = false;
                    return;

                case FindDialog.SearchModes.SearchAgain:
                    searchButton.Text = "Search &Again";
                    return;

                case FindDialog.SearchModes.SearchFailed:
                    searchButton.Text = "Not found";
                    searchButton.Enabled = false;
                    new InfoForm("Search text was not found", Owner).ShowDialog();
                    findDialog.SearchMode = FindDialog.SearchModes.Ready;
                    return;

                case FindDialog.SearchModes.SearchFinished:
                    searchButton.Text = "Not found";
                    searchButton.Enabled = false;
                    if (numberFindAllReplaces == 0)
                        new InfoForm("Finished searching document", Owner).ShowDialog();
                    findDialog.SearchMode = FindDialog.SearchModes.Ready;
                    return;
            }
        }

        /// <summary>
        /// A generaral-purpose function to serialise a list to a stream
        /// </summary>
        /// <param name="stream">The stream to write the data into</param>
        /// <param name="formatter">The formatter to use</param>
        /// <param name="list">The list to serialise</param>
        private static void SerializeList(Stream stream, IFormatter formatter, IList list)
        {
            formatter.Serialize(stream, list.Count);
            foreach (object listMember in list)
            {
                formatter.Serialize(stream, listMember);
            }
        }

        /// <summary>
        /// A general-purpose function to deserialize a list from a stream
        /// </summary>
        /// <param name="stream">The stream to read the data from</param>
        /// <param name="formatter">The formatter to use</param>
        /// <param name="list">The list to deserialize</param>
        private static void DeserializeList(Stream stream, IFormatter formatter, IList list)
        {
            int count = (int)formatter.Deserialize(stream);
            for (int idx = 0; idx != count; idx++)
            {
                list.Add(formatter.Deserialize(stream));
            }
            if (count != list.Count)
            {
                throw new Exception("List did not deserialize to correct size");
            }
        }

        /// <summary>
        /// Record the user-changable settings and apperance of the form for later restoration
        /// </summary>
        /// <param name="stream">A stream to write the data into</param>
        /// <param name="formatter">The formatter to use</param>
        internal void GetRestoreData(Stream stream, IFormatter formatter)
        {
            formatter.Serialize(stream, Bounds);
            formatter.Serialize(stream, ignoreCaseCheckBox.Checked);
            formatter.Serialize(stream, searchTypeComboBox.SelectedIndex);
            formatter.Serialize(stream, searchHistoryComboBox.Text);
            SerializeList(stream, formatter, searchHistoryComboBox.Items);
            formatter.Serialize(stream, replaceHistoryComboBox.Text);
            SerializeList(stream, formatter, replaceHistoryComboBox.Items);
            formatter.Serialize(stream, replaceModeCheckBox.Checked);
        }
        
        /// <summary>
        /// Apply the dialog settings that have previously been serialized
        /// </summary>
        /// <param name="stream">The stream to apply the settings from</param>
        /// <param name="formatter">The formatter to use</param>
        private void ApplyRestoreData(Stream stream, IFormatter formatter)
        {
            Bounds = (Rectangle)formatter.Deserialize(stream);
            ignoreCaseCheckBox.Checked = (bool)formatter.Deserialize(stream);
            searchTypeComboBox.SelectedIndex = (int)formatter.Deserialize(stream);
            searchHistoryComboBox.Text = (string)formatter.Deserialize(stream);
            DeserializeList(stream, formatter, searchHistoryComboBox.Items);
            replaceHistoryComboBox.Text = (string)formatter.Deserialize(stream);
            DeserializeList(stream, formatter, replaceHistoryComboBox.Items);
            replaceModeCheckBox.Checked = (bool)formatter.Deserialize(stream);
        }

        /// <summary>
        /// User has switched search/replace mode
        /// </summary>
        private void replaceModeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            SetAppearanceFromReplaceMode();
        }

        /// <summary>
        /// Is the dialog showing options relating to Find and Replace?
        /// </summary>
        internal bool ReplaceMode
        {
            get
            {
                return replaceModeCheckBox.Checked;
            }
            set
            {
                replaceModeCheckBox.Checked = value;
            }
        }

        /// <summary>
        /// Reposition the dialog elements to the positions appropriate to the search/replace mode
        /// </summary>
        private void SetAppearanceFromReplaceMode()
        {
            if (replaceModeCheckBox.Checked)
            {
                // dress dialog for search/replace mode
                searchButton.Location = new Point(searchButton.Location.X, 24);
                searchHistoryComboBox.Size = new Size(ClientSize.Width - 96, searchButton.Width);
                label2.Visible = true;
                replaceHistoryComboBox.Visible = true;
                replaceButton.Visible = true;
                replaceAllButton.Visible = true;
                cancelButton.Visible = true;
                MaximumSize = new Size(MaximumSize.Width, int.MaxValue);
                ClientSize = new Size(ClientSize.Width, 130);
                MinimumSize = new Size(MinimumSize.Width, Size.Height);
                MaximumSize = new Size(MaximumSize.Width, Size.Height);
                Text = "Find and Replace Text";
            }
            else
            {
                // dress dialog for search mode
                searchButton.Location = new Point(searchButton.Location.X, 54);
                searchHistoryComboBox.Size = new Size(ClientSize.Width - 12, searchHistoryComboBox.Width);
                label2.Visible = false;
                replaceHistoryComboBox.Visible = false;
                replaceButton.Visible = false;
                replaceAllButton.Visible = false;
                cancelButton.Visible = false;
                MinimumSize = new Size(MinimumSize.Width, 0);
                ClientSize = new Size(ClientSize.Width, 84);
                MinimumSize = new Size(MinimumSize.Width, Size.Height);
                MaximumSize = new Size(MaximumSize.Width, Size.Height);
                Text = "Find Text";
            }

        }
    }
}
