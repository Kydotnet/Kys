lexer grammar KysLexer;

// / fragments / //

fragment LOWER: [a-z];
fragment UPPER: [A-Z];

fragment LETTER: LOWER | UPPER;
fragment DIGIT: [0-9];

// / lang keywords / //

Kvar: 'var';

// / lang primitive values / // 

// las funciones pueden iniciar en minuscula o mayuscula
NAME: LETTER+;

//las variables siempre inician en minusculas
VAR: LOWER LETTER*;

STRING: '"' LETTER* '"';

NUMBER: '-'? DIGIT+ ('.' DIGIT+)?;

// / lang simbols / //

Sequal: '=';

//semicolon
SC: ';';

//whitespace
WS: [ \t\r\n]+ -> channel(HIDDEN);