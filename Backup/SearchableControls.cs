using System;
using System.Windows.Forms;

namespace SearchableControls
{
    /// <summary>
    /// Place holder for utility functions relating to the SearchableControls collection
    /// </summary>
    /// <remarks>
    /// <para>Part of SearchableControls written by Jim Blackler (jimblackler@gmail.com), August 2006</para>
    /// <para>Note that these functions are only worth using if you have more than one SearchableControl on 
    /// a form.</para>
    /// </remarks>
    public class Utility
    {
        /// <summary>
        /// Returns either the focused control, if it is ISearchable. Otherwise the
        /// searchable control with the lowest TabIndex.
        /// </summary>
        /// <param name="controlCollection">The control collection to search</param>
        /// <remarks>
        /// Provided as a utility function to allow client applications to easily provide a Find option in 
        /// their forms' Edit menus, or on toolbars.
        /// </remarks>
        public static ISearchable FindSearchable(Control.ControlCollection controlCollection)
        {
            ISearchable firstSearchable = null; // keep a record of the first searchable control found

            // Look at each child control to find a focused, searchable control
            foreach (Control control in controlCollection)
            {
                ISearchable searchable = control as ISearchable;
                if (searchable != null)
                {
                    if (control.Focused)
                    {
                        return searchable;
                    }
                    else if (firstSearchable == null || control.TabIndex < ((Control)firstSearchable).TabIndex)
                    {
                        firstSearchable = searchable;
                    }
                }
            }

            // No controls were focused.. return the control with the lowest TabIndex
            return firstSearchable;
        }

        /// <summary>
        /// Opens the find dialog on the most appropriate control in the container
        /// </summary>
        /// <returns>'True' if a find dialog was found to open</returns>
        /// <remarks>
        /// Uses the utility functions FindSearchable() with the form's ControlCollection to find either the
        /// focused control, if it is ISearchable, or the searchable control with the lowest TabIndex.
        /// Calls OpenDialog on the resulting control.
        /// </remarks>
        public static bool OpenFindDialog(Control.ControlCollection controls)
        {
            ISearchable searchable = Utility.FindSearchable(controls);
            if (searchable != null)
            {
                searchable.FindDialog.Show();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Calls FindNext() on the most appropriate control in the container
        /// </summary>
        /// <returns>'True' if FindNext executed successfully on a control</returns>
        /// <remarks>
        /// Uses the utility functions FindSearchable() with the form's ControlCollection to find either the
        /// focused control, if it is ISearchable, or the searchable control with the lowest TabIndex.
        /// Calls OpenDialog on the resulting control.
        /// </remarks>
        public static bool FindNext(Control.ControlCollection controls)
        {
            ISearchable searchable = Utility.FindSearchable(controls);
            if (searchable != null)
            {
                searchable.FindDialog.FindNext();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Informs the user if FindNext is available in the supplied control selection
        /// </summary>
        /// <returns>The value of FindNextIsAvailable() on the most appropriate control in the container</returns>
        /// <remarks>
        /// Uses the utility functions FindSearchable() with the form's ControlCollection to find either the
        /// focused control, if it is ISearchable, or the searchable control with the lowest TabIndex.
        /// Calls OpenDialog on the resulting control.
        /// </remarks>
        public static bool FindNextIsAvailable(Control.ControlCollection controls)
        {
            ISearchable searchable = Utility.FindSearchable(controls);
            if (searchable != null)
            {
                return searchable.FindDialog.FindNextIsAvailable();
            }
            return false;
        }
    }
}
