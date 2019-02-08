using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;
using System.ComponentModel.Design;
using System.Runtime.Serialization.Formatters.Binary;

namespace SearchableControls
{
    /// <summary>
    /// A dialog for the user to search another control or form for specified text
    /// </summary>
    public partial class FindDialog : Component
    {
        /// <summary>
        /// Record of the parent form
        /// </summary>
        [Browsable(false)]
        [DefaultValue("")]
        public Control ParentControl
        {
            get
            {
                // Designer code ... set the form 
                if ((parentControl == null) && DesignMode)
                {
                    IDesignerHost host = (IDesignerHost)GetService(typeof(IDesignerHost));
                    if (host != null)
                    {
                        parentControl = host.RootComponent as Control;
                    }

                }
                return parentControl;
            }
            set
            {
                if (parentControl != value)
                {
                    // Quite a bit of setup is done as soon as we know the parent form

                    parentControl = value;

                    parentControl.Leave += new EventHandler(parentControl_Leave);
                }
            }
        }

        private Control parentControl;

        /// <summary>
        /// Event added to the parent control so that the FindForm can hide itself when the parent control isn't active
        /// </summary>
        void parentControl_Leave(object sender, EventArgs e)
        {
            if (findForm != null)
            {
                findForm.Hide();
            }
        }

        private FindForm findForm;

        /// <summary>
        /// Make a dialog for the user to search for specified text in a control or form
        /// </summary>
        public FindDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handle deactivate instructions received from FindForm
        /// </summary>
        /// <param name="sender">Sending object</param>
        /// <param name="e">Event args</param>
        void findForm_Deactivate(object sender, EventArgs e)
        {
            if (Deactivate != null)
            {
                Deactivate(sender, e);
            }
        }

        /// <summary>
        /// A local store of the FindForm's restore data (user-changable settings)
        /// </summary>
        private Stream formRestoreData;

        /// <summary>
        /// Present the form to the user with the given default text
        /// </summary>
        /// <param name="defaultText">The default text to set</param>
        /// <param name="replaceMode">Start in Replace mode?</param>
        public void Show(string defaultText, bool replaceMode)
        {
            if (findForm == null) // Create the form if it doesn't exist already
            {
                if (formRestoreData != null)
                {
                    formRestoreData.Seek((long)0, SeekOrigin.Begin);
                }
                findForm = new FindForm(this, formRestoreData, new BinaryFormatter(), defaultText, replaceMode, replaceMode || ReplaceAvailable);
                findForm.Deactivate += new EventHandler(findForm_Deactivate);
                findForm.Closing += new CancelEventHandler(findForm_Closing);
            }
            findForm.Reshow(); // Re-show the form

            if (findForm.ReplaceMode != replaceMode)
            {
                findForm.ReplaceMode = replaceMode;
            }

        }

        /// <summary>
        /// Present the form to the user with the given default text
        /// </summary>
        /// <param name="defaultText">The default text to set</param>
        public void Show(string defaultText)
        {
            Show(defaultText, false);
        }

        /// <summary>
        /// Present the form to the user
        /// </summary>
        /// <param name="replaceMode">Start in Replace mode</param>
        public void Show(bool replaceMode)
        {
            Show(null, replaceMode);
        }

        /// <summary>
        /// Present the form to the user
        /// </summary>
        public void Show()
        {
            Show(null, false);
        }


        /// <summary>
        /// Is the find dialog currently visible?
        /// </summary>
        public bool Visible
        {
            get
            {
                return findForm != null;
            }
        }

        /// <summary>
        /// Close the FindDialog - whatever state it is in
        /// </summary>
        public void Close()
        {
            findForm.Close();
        }

        /// <summary>
        /// Instigate a search
        /// </summary>
        /// <remarks>
        /// Intended for use by FindDialog or FindForm only
        /// </remarks>
        internal bool Search()
        {
            // Set up new event args. 'First' flag is set depending on the display mode of the find form
            SearchEventArgs eventArgs = new SearchEventArgs(SearchRegularExpression, SearchMode == FindDialog.SearchModes.Ready);
            if (searchRequested == null)
            {
                throw new Exception("No search event supplied");
            }
            searchRequested(this, eventArgs);

            // set the state of the search form depending on the result of the search
            if (eventArgs.Successful)
            {
                SearchMode = SearchModes.SearchAgain;
                return true;
            }
            if (eventArgs.FirstSearch)
            {
                SearchMode = SearchModes.SearchFailed;
            }
            else
            {
                SearchMode = SearchModes.SearchFinished;
            }
            return false;
        }

        /// <summary>
        /// Returns true if FindNext is available
        /// </summary>
        public bool FindNextIsAvailable()
        {
            return (SearchMode == FindDialog.SearchModes.SearchAgain);
        }

        /// <summary>
        /// Makes the dialog instigate another search - if this facility is available
        /// </summary>
        /// <returns>True if the search was succesful</returns>
        public bool FindNext()
        {
            if (FindNextIsAvailable())
            {
                return Search();
            }

            return false;
        }

        /// <summary>
        /// The states that the search system can be in
        /// </summary>
        internal enum SearchModes
        {
            NullState,
            Ready,
            SearchAgain,
            SearchFailed,
            SearchFinished
        }

        /// <summary>
        /// The current state of the search system from the user's point of view
        /// </summary>
        internal SearchModes SearchMode
        {
            get
            {
                return searchMode;
            }
            set
            {
                if (searchMode != value)
                {
                    FindDialog.SearchModes modes1 = SearchMode;
                    searchMode = value;
                    if (findForm != null)
                    {
                        findForm.ApplySearchMode();
                    }
                }
            }
        }

        private SearchModes searchMode;

        /// <summary>
        /// The most recent search requested in regular expression form
        /// </summary>
        public Regex SearchRegularExpression
        {
            get
            {
                return searchRegularExpression;
            }
            set
            {
                searchRegularExpression = value;
            }
        }

        private Regex searchRegularExpression;

        /// <summary>
        /// When the find form is being closed we record the user-changable settings for later restoration
        /// </summary>
        private void findForm_Closing(object sender, CancelEventArgs e)
        {
            formRestoreData = new MemoryStream();
            findForm.GetRestoreData(formRestoreData, new BinaryFormatter());
            findForm = null;
            ParentControl.Focus(); // This is required if an InforForm has been shown.
            // Otherwise, focus is orphaned. I am not sure why.
        }

        /// <summary>
        /// Is Replace offered on the find dialog if just Find is selected?
        /// </summary>
        public bool ReplaceAvailable
        {
            get
            {
                return replaceAvailable;
            }
            set
            {
                replaceAvailable = value;
            }
        }

        private bool replaceAvailable = false;

        /// <summary>
        /// The find dialog is no longer active
        /// </summary>
        public event EventHandler Deactivate;

        /// <summary>
        /// The find dialog is requesting a search
        /// </summary>
        public event SearchEventHandler SearchRequested
        {
            add
            {
                searchRequested += value;
            }
            remove
            {
                searchRequested -= value;
            }

        }

        private SearchEventHandler searchRequested;

        /// <summary>
        /// The find dialog is requesting a replace of the last selected text
        /// </summary>
        public event ReplaceEventHandler ReplaceRequested
        {
            add
            {
                replaceRequested += value;
            }
            remove
            {
                replaceRequested -= value;
            }

        }

        private ReplaceEventHandler replaceRequested;

        /// <summary>
        /// The find dialog is requesting a cancel of the last replace operation
        /// </summary>
        public event EventHandler CancelReplaceRequested
        {
            add
            {
                cancelReplaceRequested += value;
            }
            remove
            {
                cancelReplaceRequested -= value;
            }

        }

        private EventHandler cancelReplaceRequested;



        /// <summary>
        /// Start a replace operation on the last selected text
        /// </summary>
        /// <param name="replaceText">The string to replace</param>
        internal void Replace(string replaceText)
        {
            ReplaceEventArgs args = new ReplaceEventArgs();
            args.ReplaceText = replaceText;

            if(replaceRequested==null)
            {
                throw new Exception("No replace event handler supplied");
            }
            replaceRequested(this, args);
        }

        internal void CancelReplace()
        {
            if (cancelReplaceRequested == null)
            {
                throw new Exception("No replace event handler supplied");
            }
            cancelReplaceRequested(this, EventArgs.Empty);
        }

        /// <summary>
        /// Does the find dialog have the user focus?
        /// </summary>
        public bool Focused
        {
            get
            {
                if (findForm != null)
                {
                    return findForm.Focused;
                }
                else return false;
            }
        }
    }
}
