namespace MonoKle.Script.Common.Internal
{
    using System;

    internal static class ByteCodeValues
    {
        // Returns
        internal const byte OP_RETURN_VOID = 0x01;
        internal const byte OP_RETURN_VALUE = 0x02;

        // Arithmetic operators
        internal const byte OP_ADD = 0x03;
        internal const byte OP_SUBTRACT = 0x04;
        internal const byte OP_MULTIPLY = 0x05;
        internal const byte OP_DIVIDE = 0x06;
        internal const byte OP_MODULO = 0x07;
        internal const byte OP_POWER = 0x08;
        internal const byte OP_NEGATE = 0x09;

        // Logical operators
        internal const byte OP_EQUAL = 0x10;
        internal const byte OP_NOTEQUAL = 0x11;
        internal const byte OP_LARGER = 0x12;
        internal const byte OP_LARGEREQUAL = 0x13;
        internal const byte OP_SMALLER = 0x14;
        internal const byte OP_SMALLEREQUAL = 0x15;
        internal const byte OP_NOT = 0x16;
        internal const byte OP_AND = 0x17;
        internal const byte OP_OR = 0x18;

        // Built in functions
        internal const byte OP_PRINT = 0x20;

        // Flow Control
        internal const byte OP_IF = 0x30;
        internal const byte OP_JUMP = 0x31;

        // Object interaction
        internal const byte OP_INIVAR_READOBJECT = 0x40;
        internal const byte OP_SETVAR_READOBJECT = 0x41;
        internal const byte OP_READOBJECT_FIELDPROPERTY = 0x42;
        internal const byte OP_READOBJECT_FUNCTION = 0x43;
        internal const byte OP_WRITEOBJECT_FIELDPROPERTY = 0x44;
        internal const byte OP_NEWOBJECT = 0x45;

        // Types        
        internal const byte TYPE_BOOL = 0x50;
        internal const byte TYPE_INT = 0x51;
        internal const byte TYPE_FLOAT = 0x52;
        internal const byte TYPE_STRING = 0x53;
        internal const byte TYPE_OBJECT = 0x54;

        // Function
        internal const byte OP_CALLFUNCTION = 0xD0;

        // Initiate/Get/Set variable operations
        internal const byte OP_INIVAR = 0xE0;
        internal const byte OP_SETVAR = 0xE1;
        internal const byte OP_GETVAR = 0xE2;
        internal const byte OP_REMVAR = 0xE3;

        // Constant operations
        internal const byte OP_CONST_BOOL = 0xFF;
        internal const byte OP_CONST_INT = 0xFE;
        internal const byte OP_CONST_FLOAT = 0xFD;
        internal const byte OP_CONST_STRING = 0xFC;
        internal const byte OP_CONST_OBJECT = 0xFB;
    }
}
