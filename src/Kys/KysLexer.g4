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

Kvar: 'var'; // / lang keywords / //

Kfunc: 'func';

// / lang values / // 

BOOL: 'true' | 'false';

VAR: LOWER+;

GVAR: Sdolar VAR;

RVAR: Sarr VAR;

CONST: UPPER+;

FUNC: LETTER+;

STRING: '"' (LETTER | DIGIT | SPACE | PATH | SYMBOL)* '"';

NUMBER: '-'? DIGIT+ ('.' DIGIT+)?;

ANDOR: Sor | Sand;

// / lang simbols / //

Scomma: ',';

Sor: '||';

Sand: '&&';

Snot: '~' | '!';

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