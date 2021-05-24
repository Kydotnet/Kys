grammar Kys;

options {
	tokenVocab = KysLexer;
}

program: (instruction)+;

instruction: exitprogram | sentence;

exitprogram: Kexit NUMBER SC;

sentence: control | funccall | declaration | asignation;

control:
	ifcontrol
	| whilecontrol
	| twhilecontrol
	| waitcontrol
	| forcontrol;

ifcontrol: Kif SLpar expression SRpar block elsecontrol?;

elsecontrol: Kelse (ifcontrol | block);

whilecontrol: Kwhile SLpar expression SRpar block;

twhilecontrol: Ktimed Kwhile twbucle;

waitcontrol: Kwait twbucle;

twbucle:
	SLpar expression Scomma NUMBER SRpar block timeoutcontrol?;

timeoutcontrol: Ktimeout SLpar NUMBER SRpar block;

forcontrol:
	Kfor SLpar varoperation expression SC NUMBER SRpar block;

varoperation: declaration | asignation;

block: SLbrack sentence* SRbrack;

funccall: funcresult SC;

funcresult: ID SLpar arguments? SRpar;

arguments: expression (Scomma expression)*;

declaration: Kvar asignation;

asignation: ID Sequal expression SC;

expression:
	SLpar expression SRpar								# parenthesisExp
	| <assoc = right> Snot expression					# uniNotExp
	| <assoc = right> expression UNIARIT				# uniAritExp
	| <assoc = right> expression POTENCIAL expression	# potencialExp
	| expression MULTIPLICATIVE expression				# multiplicativeExp
	| expression ADITIVE expression						# aditiveExp
	| expression EQUALITY expression					# equalityExp
	| expression ANDOR expression						# logicalExp
	| funcresult										# funcExp
	| value												# valueExp;

value: STRING | NUMBER | BOOL | ID;


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

Kuse: 'use';

Klib: 'lib';

Kkys: 'kys';

// / lang values / // 

NULL: 'null';

BOOL: 'true' | 'false';

ID: LETTER+;

GID: Sdolar ID;

RID: Sarr ID;

STRING: '"' ANY*? '"';

NUMBER: '-'? DIGIT+ ('.' DIGIT+)?;

// / lang operators / //

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

SC: ';';

WS: [ \t\r\n]+ -> channel(HIDDEN);

ALL: .;
