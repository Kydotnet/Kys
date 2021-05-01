lexer grammar KysLexer;

fragment LOWER: [a-z];
fragment UPPER: [A-Z];

fragment LETTER: LOWER | UPPER;
fragment DIGIT: [0-9];

// las funciones pueden iniciar en minuscula o mayuscula
NAME: LETTER+;

//las variables siempre inician en minusculas
VAR: LOWER LETTER*;

NUMBER: '-'? DIGIT+ ('.' DIGIT+)?;

WHITESPACE: ' ' -> channel(HIDDEN);