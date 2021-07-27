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

Kset: 'set';

Kdef: 'def';

Kfunc: 'func';

Kexit: 'exit';

Kif: 'if';

Kelse: 'else';

Kwhile: 'while';

Ktimed: 'timed';

Ktimeout: 'timeout';

Kwait: 'wait';

Kfor: 'for';

Kkyl: 'kyl';

Kkys: 'kys';

// / lang values / // 

NULL: 'null';

BOOL: 'true' | 'false';

ID: LETTER+;

GID: Sdolar (DIGIT+ | Smul);

RID: Sarr ID;

STRING: '"' ANY*? '"';

NUMBER: '-'? DIGIT+ ('.' DIGIT+)?;

// / lang operators / //

UNIARIT: Splus Splus | Sminus Sminus;

POTENCIALASSIGN: POTENCIAL Sequal;

POTENCIAL: Spot | Sroot;

MULTIPLICATIVEASSIGN: MULTIPLICATIVE Sequal;

MULTIPLICATIVE: Smul | Sdiv;

MODULEASSIGN: Smod Sequal;

ADITIVEASSIGN: ADITIVE Sequal;

ADITIVE: Splus | Sminus;

EQRELATIONAL: RELATIONAL Sequal;

RELATIONAL: Sless | Sgreat;

EQUALITY: Sequal Sequal | Snot Sequal;

ANDOR: Sand | Sor;

PARAMS: Sdot Sdot Sdot;

// / lang simbols / //

Sless: '<';

Sgreat: '>';

Smod: '%';

Spot: '^';

Sroot: '~';

Sdiv: '/';

Smul: '*';

Splus: '+';

Sminus: '-';

Scomma: ',';

Sdot: '.';

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

SC: ';';

WS: [ \t\r\n]+ -> channel(HIDDEN);

ALL: .;