namespace MonoKleScript.VM.Event
{
    /// <summary>
    /// Event handler for runtime error events.
    /// </summary>
    /// <param name="sender">Sender of the event.</param>
    /// <param name="e">Event arguments.</param>
    public delegate void RuntimeErrorEventHandler(object sender, RuntimeErrorEventArgs e);
}