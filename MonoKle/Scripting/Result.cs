using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoKle.Scripting
{
    public struct Result
    {
        public bool sucess;
        public Type returnType;
        public object returnValue;

        public Result(bool sucess, Type returnType, object returnValue)
        {
            this.sucess = sucess;
            this.returnType = returnType;
            this.returnValue = returnValue;
        }

        public static Result Fail
        {
            get { return new Result(false, null, null); }
        }
    }
}
