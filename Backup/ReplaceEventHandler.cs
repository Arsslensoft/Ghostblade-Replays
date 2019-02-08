using System;

namespace SearchableControls
{
    /// <summary>
    /// The search dialog is requesting a replace of the last selected text
    /// </summary>
    /// <param name="sender">Sender object (the search dialog)</param>
    /// <param name="e">Replace event parameters</param>
    public delegate void ReplaceEventHandler(object sender, ReplaceEventArgs e);
}
