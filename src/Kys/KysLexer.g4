lexer grammar KysLexer;

// / instructions / //

BASH: Sinstruction ANY+ -> channel(HIDDEN);

COMMENT: Scomment ANY* -> channel(HIDDEN);

// / fragments / //

fragment LOWER: [a-z];

fragment UPPER: [A-Z];

fragment LETTER: LOWER | UPPER;

fragment DIGIT: [0-9];

fragment ANY: ~[\r\n];

// / lang keywords / //

Kvar: 'var';

Kfunc: 'func';

Kexit: 'exit';

Kif: 'if';

Kelse: 'else';

Kwhile: 'while';

Ktimed: 'timed';

Ktimeout: 'timeout';

Kwait: 'wait';

Kfor: 'for';

// / lang values / // 

BOOL: 'true' | 'false';

ID: LETTER+;

GID: Sdolar ID;

RID: Sarr ID;

STRING: '"' ANY*? '"';

NUMBER: '-'? DIGIT+ ('.' DIGIT+)?;

UNIARIT: Splus Splus | Sminus Sminus;

POTENCIAL: Spot | Sroot;

MULTIPLICATIVE: Smul | Sdiv;

ADITIVE: Splus | Sminus;

EQUALITY: Sequal Sequal | Snot Sequal;

ANDOR: Sand | Sor;

// / lang simbols / //

Spot: '^';

Sroot: '~';

Sdiv: '/';

Smul: '*';

Splus: '+';

Sminus: '-';

Scomma: ',';

Sor: '||';

Sand: '&&';

Snot: '!';

SRpar: ')';

SLpar: '(';

SLbrack: '{';

SRbrack: '}';

Sequal: '=';

Sdolar: '$';

Sarr: '@';

Scomment: '//';

Sinstruction: '#!';

//semicolon
SC: ';';

//whitespace
WS: [ \t\r\n]+ -> channel(HIDDEN);

SPACE: [ \t]+;

ALL: .;