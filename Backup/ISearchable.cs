using System;

namespace SearchableControls
{
    /// <summary>
    /// Searchable controls inherit from ISearchable interface simply to make it easier to invoke
    /// searches from a form's menu or toolbars when multiple searchable controls are used.
    /// </summary>
    public interface ISearchable
    {
        /// <summary>
        /// Return the FindDialog (if any) currently associated with this control
        /// </summary>
        FindDialog FindDialog { get; }
    }
}
