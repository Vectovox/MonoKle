namespace MonoKleScript.VM
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public struct Result
    {
        public bool success;
        public Type returnType;
        public object returnValue;

        public Result(bool success, Type returnType, object returnValue)
        {
            this.success = success;
            this.returnType = returnType;
            this.returnValue = returnValue;
        }

        public static Result Fail
        {
            get { return new Result(false, null, null); }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("Success: ");
            sb.Append(success);
            if (success)
            {
                sb.Append(" | Type: ");
                sb.Append(this.returnType.ToString());
                sb.Append(" | Value: ");
                sb.Append(this.returnValue);
            }
            return sb.ToString();
        }
    }
}