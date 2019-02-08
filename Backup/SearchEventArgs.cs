using System;
using System.Text.RegularExpressions;

namespace SearchableControls
{
    /// <summary>
    /// Search event arguments for a control search
    /// </summary>
    public class SearchEventArgs : EventArgs
    {
        /// <summary>
        /// Whether or not the search has returned a match
        /// </summary>
        /// <remarks>To be set by the </remarks>
        public bool Successful
        {
            get { return successful; }
            set { successful = value; }
        }

        private bool successful = false;

        /// <summary>
        /// Whether or not the search restarted from the top of the document(for the find dialog to reflect)
        /// </summary>
        public bool RestartedFromDocumentTop
        {
            get { return restartedFromDocumentTop; }
            set { restartedFromDocumentTop = value; }
        }

        private bool restartedFromDocumentTop = false;

        /// <summary>
        /// 'True' if this is the first search performed
        /// </summary>
        /// <remarks>
        /// Optional, to be used by the searching control to remember the selection position 
        /// when searching begins.
        /// </remarks>
        public bool FirstSearch
        {
            get { return firstSearch; }
        }

        private bool firstSearch;

        /// <summary>
        /// The regular expression that performs the search
        /// </summary>
        public Regex SearchRegularExpression
        {
            get { return searchRegularExpression; }
        }

        private Regex searchRegularExpression;

        /// <summary>
        /// Makes search event arguments for a control search
        /// </summary>
        public SearchEventArgs(Regex _searchRegularExpression, bool _firstSearch)
        {
            searchRegularExpression = _searchRegularExpression;
            firstSearch = _firstSearch;
        }
    }
}
