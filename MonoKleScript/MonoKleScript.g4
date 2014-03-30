grammar MonoKleScript;

options {
    language=CSharp_v4_0;
}

/* ### RULES ### */
// Structure and flow
script : block;

block : blockstatements
      | /* EPSILON */
      ;

blockstatements : statement
                | statement blockstatements;

statement : assignment
          | initialization
        //| IF expression THEN block ENDIF # if
          | function
          | keyReturn
          | keyPrint
          ;

// Variables
assignment : IDENTIFIER ASSIGNMENT expression;
initialization : TYPE IDENTIFIER ASSIGNMENT expression;

// Keyword commands
keyPrint : PRINT expression;

keyReturn : RETURN
          | RETURN expression
          ;

// Expression
expression : LGROUPING expression RGROUPING # ExpGrouping
           | NOT expression # ExpNot
           | MINUS expression # ExpNegate
           | expression POWER expression # ExpPower
           | expression DIVIDE expression # ExpDivide
           | expression MULTIPLY expression # ExpMultiply
           | expression MODULO expression # ExpModulo
           | expression PLUS expression # ExpPlus
           | expression MINUS expression # ExpMinus
           | expression SMALLER expression #ExpSmaller
           | expression SMALLEREQUALS expression #ExpSmallerEquals
           | expression GREATER expression #ExpGreater
           | expression GREATEREQUALS expression #ExpGreaterEquals
           | expression EQUALS expression #ExpEquals
           | expression NOTEQUALS expression #ExpNotEquals
           | expression AND expression # ExpAnd
           | expression OR expression # ExpOr
           | value # ExpValue
           ;

// Value
value : INT # ValueInt
      | FLOAT # ValueFloat
      | STRING # ValueString
      | BOOL # ValueBool
      | IDENTIFIER # ValueVariable
      | function # ValueFunction
      ;

// Function
function : IDENTIFIER LGROUPING RGROUPING
         | IDENTIFIER LGROUPING parameters RGROUPING
         ;

parameters : expression
           | expression PARAMDIVIDER parameters
           ;



/* ### TOKENS ### */
TYPE : 'int' | 'float' | 'string';
ASSIGNMENT : ':';

IF : 'if';
THEN : 'then';
ENDIF : 'endif';

RETURN : 'return';
PRINT : 'print';

PLUS : '+';
MINUS : '-';
MULTIPLY : '*';
DIVIDE : '/';
MODULO : '%';
POWER : '^';

NOT : 'not';
AND : 'and';
OR : 'or';
EQUALS : 'is';
NOTEQUALS : 'is not';
GREATER : 'is greater than';
GREATEREQUALS : 'is greater than or is';
SMALLER : 'is smaller than';
SMALLEREQUALS : 'is smaller than or is';

LGROUPING : '(';
RGROUPING : ')';

WHITESPACE :  (' '|'\t'|'\r'|'\n')+ -> skip;

BLOCKCOMMENT :   '/*' .*? '*/' -> skip;
COMMENT : '//' .*? '\n' -> skip;

BOOL : ('true' | 'false');
INT : DIGIT+;
FLOAT : DIGIT+ '.' DIGIT+;
STRING :   '"' ( ESCAPED_QUOTE | . )*? '"';

IDENTIFIER : LETTER+ (LETTER | DIGIT | '_')*;

PARAMDIVIDER : ',';

fragment DIGIT : '0'..'9';
fragment LETTER : ('a'..'z' | 'A'..'Z');
fragment ESCAPED_QUOTE : '\\"';
