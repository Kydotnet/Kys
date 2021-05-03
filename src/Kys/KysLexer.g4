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

fragment PATH: [\\/:.];

// / lang keywords / //

Kvar: 'var';

Kfunc: 'func';

Kexit: 'exit';

// / lang values / // 

BOOL: 'true' | 'false';

VAR: LOWER+;

GVAR: Sdolar VAR;

RVAR: Sarr VAR;

CONST: UPPER+;

FUNC: LETTER+;

STRING: '"' (LETTER | DIGIT | SPACE | PATH | SYMBOL)* '"';

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

SYMBOL:
	Scomma
	| Sor
	| Sand
	| Snot
	| SRpar
	| SLpar
	| SLbrack
	| SRbrack
	| Sequal
	| Sdolar
	| Sarr;

//whitespace
WS: [ \t\r\n]+ -> channel(HIDDEN);

SPACE: [ \t]+;

ALL: .;