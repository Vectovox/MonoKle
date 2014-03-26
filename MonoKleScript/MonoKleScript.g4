grammar MonoKleScript;

options {
    language=CSharp_v4_0;
}

/* ### RULES ### */
// Structure and flow
script : block;

block : statement
      | statement statement
      | /* EPSILON */
      ;

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
           | expression DIVIDE expression # ExpDivide
           | expression MULTIPLY expression # ExpMultiply
           | expression PLUS expression # ExpPlus
           | expression MINUS expression # ExpMinus
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

NOT : '!';

LGROUPING : '(';
RGROUPING : ')';

WHITESPACE :  (' '|'\t'|'\r'|'\n')+ -> skip;

BOOL : ('true' | 'false');
INT : DIGIT+;
FLOAT : DIGIT+ '.' DIGIT+;
STRING :   '"' ( ESCAPED_QUOTE | . )*? '"';

IDENTIFIER : LETTER+ (LETTER | DIGIT | '_')*;

PARAMDIVIDER : ',';

fragment DIGIT : '0'..'9';
fragment LETTER : ('a'..'z' | 'A'..'Z');
fragment ESCAPED_QUOTE : '\\"';
