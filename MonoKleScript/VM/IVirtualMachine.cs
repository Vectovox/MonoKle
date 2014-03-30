namespace MonoKleScript.VM
{
    using MonoKleScript.Script;
    using MonoKleScript.VM.Event;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IVirtualMachine
    {
        Result ExecuteScript(string script);

        Result ExecuteScript(string script, object[] arguments);

        int LoadScripts(ICollection<ByteScript> scripts);

        event PrintEventHandler Print;

        event RuntimeErrorEventHandler RuntimeError;
    }
}
