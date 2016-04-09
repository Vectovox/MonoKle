using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoKle.Scripting
{
    public class ScriptCompilationError
    {
        public string Message { get; set; }

        public int Line { get; set; }

        public bool IsWarning { get; set; }

        public override string ToString()
        {
            return $"Line {Line} - {Message}";
        }
    }
}
