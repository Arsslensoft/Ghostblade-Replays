using System;

namespace SearchableControls
{
    /// <summary>
    /// Search event arguments for a control replace operation
    /// </summary>
    public class ReplaceEventArgs : EventArgs
    {
        /// <summary>
        /// The text to replace the most recent selection
        /// </summary>
        public string ReplaceText
        {
            get { return replaceText; }
            set { replaceText = value; }
        }

        private string replaceText;
    }
}
