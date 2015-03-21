using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoKle.Networking
{
    public abstract class AbstractConnection
    {
        public abstract bool IsConnected { get; }
    }
}
