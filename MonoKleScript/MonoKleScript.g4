grammar MonoKleScript;

options {
    language=CSharp_v5_0;
}

/* ### RULES ### */
// Structure and flow
script : block;

block : blockstatements
      | /* EPSILON */
      ;

blockstatements : statement
                | statement blockstatements;

statement : initialization_readobject
          | assignment_readobject
		  | assignment_writeobject
          | assignment
          | initialization
          | conditional
		  | while
          | function
		  | objectfunction
          | keyReturn
          | keyPrint
          ;

while : WHILE expression DO block ENDWHILE;

conditional : IF if elseif else ENDIF;
if : expression THEN block;
elseif : ELSEIF if elseif
       | /* EPSILON */
       ;
else : ELSE block
     | /* EPSILON */
	 ;

// Variables
initialization_readobject : TYPE IDENTIFIER OBJECTREAD objectvalue;
assignment_readobject : IDENTIFIER OBJECTREAD objectvalue;
assignment_writeobject : IDENTIFIER OBJECTACCESS IDENTIFIER OBJECTREAD expression;
initialization : TYPE IDENTIFIER ASSIGNMENT expression;
assignment : IDENTIFIER ASSIGNMENT expression;

// Object
objectvalue : IDENTIFIER OBJECTACCESS IDENTIFIER # OV
            | objectfunction #OF
			;

objectfunction: IDENTIFIER OBJECTACCESS IDENTIFIER LGROUPING RGROUPING
		      | IDENTIFIER OBJECTACCESS IDENTIFIER LGROUPING parameters RGROUPING
			  ;

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
TYPE : 'int' | 'float' | 'string' | 'object';
ASSIGNMENT : ':';

IF : 'if';
THEN : 'then';
ELSE : 'else';
ELSEIF : 'else if';
ENDIF : 'endif';
WHILE : 'while';
DO : 'do';
ENDWHILE : 'endwhile';

RETURN : 'return';
PRINT : 'print';

OBJECTACCESS : '.';
OBJECTREAD : '<-';

PLUS : '+';
MINUS : '-';
MULTIPLY : '*';
DIVIDE : '/';
MODULO : '%';
POWER : '^';

NOT : 'not';
AND : 'and';
OR : 'or';
EQUALS : '=';
NOTEQUALS : '!=';
GREATER : '>';
GREATEREQUALS : '>=';
SMALLER : '<';
SMALLEREQUALS : '<=';

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
