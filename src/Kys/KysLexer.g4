lexer grammar KysLexer;

// / instructions / //

COMMENT: (Scomment (SPACE | LETTER | DIGIT)+) -> channel(HIDDEN);

// / fragments / //

fragment LOWER: [a-z];
fragment UPPER: [A-Z];

fragment LETTER: LOWER | UPPER;
fragment DIGIT: [0-9];

// / lang keywords / //

Kvar: 'var';

Kfunc: 'func';

// / lang values / // 

VAR: LOWER+;

GVAR: Sdolar VAR;

RVAR: Sarr VAR;

CONST: UPPER+;

STRING: '"' LETTER* '"';

NUMBER: '-'? DIGIT+ ('.' DIGIT+)?;

// / lang simbols / //

Sequal: '=';

Sdolar: '$';

Sarr: '@';

Scomment: '//';

//semicolon
SC: ';';

//whitespace
WS: [ \t\r\n]+ -> channel(HIDDEN);

SPACE: [ \t]+;