namespace MonoKle.Script.Compiler.Event
{
    /// <summary>
    /// Event handler for semantics error events.
    /// </summary>
    /// <param name="sender">Sender of the event.</param>
    /// <param name="e">Event arguments.</param>
    public delegate void SemanticErrorEventHandler(object sender, SemanticErrorEventArgs e);
}
