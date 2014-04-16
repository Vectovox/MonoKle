﻿namespace MonoKle.Script.Compiler.Event
{
    /// <summary>
    /// Event handler for compilation error events.
    /// </summary>
    /// <param name="sender">Sender of the event.</param>
    /// <param name="e">Event arguments.</param>
    public delegate void CompilationErrorEventHandler(object sender, CompilationErrorEventArgs e);
}