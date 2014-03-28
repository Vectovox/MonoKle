namespace MonoKleScript.IO.Error
{
    /// <summary>
    /// Event handler for script reading error events.
    /// </summary>
    /// <param name="sender">Sender of the event.</param>
    /// <param name="e">Event arguments.</param>
    public delegate void ScriptReadingErrorEventHandler(object sender, ScriptReadingErrorEventArgs e);
}