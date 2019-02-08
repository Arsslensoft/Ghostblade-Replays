using System;
using System.Collections.Generic;
using System.Text;

namespace SearchableControls
{
    /// <summary>
    /// The search dialog is requesting a search
    /// </summary>
    /// <param name="sender">Sender object</param>
    /// <param name="e">Search event parameters</param>
    /// <remarks>
    /// <para>It is intended for the searchable control or form to supply a handler for this event.</para>
    /// <para>It is expected to search its own content from the currently selected position to the end of the document or to 
    /// to the originally selected position (at the beginning of the search when e.FirstSearch was true), whichever is first.</para>
    /// <para> If a match is found it is expected to select the match and set e.Succesful to be true. If no match is found,
    /// and the originally selected position is before the current position, the control is expected to set
    /// e.RestartedFromDocumentTop to be True and then search the document from the top to the originally selected position.</para>
    /// <para>When finding a match, the control is expected to visibly select the found text in a manner that will still be
    /// visible when the form of FindDialog is in focus.</para>
    /// <para>If no match is found by either technique the control is expected to leave Succesful set as false.</para>
    /// </remarks>
    public delegate void SearchEventHandler(object sender, SearchEventArgs e);
}
