namespace MonoKle.Scripting
{
    using System;

    internal struct Token
    {
        public string text;
        public byte hiearchyValue;
        public bool isLogicOperator;
        public bool isArithmeticOperator;
        public bool isFunction;
        public bool isOperand;
        public bool isGroupingLeft;
        public bool isGroupingRight;
        public Type operandType;
        public string functionName;
        public Token[][] functionArguments;
        
        public bool IsOperator()
        {
            return this.isArithmeticOperator || this.isLogicOperator;
        }

        public bool IsGrouping()
        {
            return this.isGroupingLeft || this.isGroupingRight;
        }

        public bool IsOperatorGrouping()
        {
            return this.IsOperator() || this.IsGrouping();
        }
    }
}
