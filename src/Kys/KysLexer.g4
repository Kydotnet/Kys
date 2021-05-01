lexer grammar KysLexer;

// / fragments / //

fragment LOWER: [a-z];
fragment UPPER: [A-Z];

fragment LETTER: LOWER | UPPER;
fragment DIGIT: [0-9];

// / lang keywords / //

Kvar: 'var';

// / lang primitive values / // 

VAR: LOWER+;

CONST: UPPER+;

STRING: '"' LETTER* '"';

NUMBER: '-'? DIGIT+ ('.' DIGIT+)?;

// / lang simbols / //

Sequal: '=';

//semicolon
SC: ';';

//whitespace
WS: [ \t\r\n]+ -> channel(HIDDEN);