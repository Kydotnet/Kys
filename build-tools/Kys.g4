grammar Kys;

program: (toplevel)* (instruction)+;

toplevel: kyl | kys;

kyl: Kkyl ID STRING+;

kys: Kkys ID value*;

instruction: exitprogram | funcdefinition | sentence;

funcdefinition: Kfunc ID SLpar parameters SRpar block;

parameters: params? PARAMS?;

params: ID (Scomma ID)*;

exitprogram: Kexit NUMBER SC;

sentence: control | funccall | varoperation SC;

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
	Kfor SLpar varoperation? SC expression? SC expression? SRpar block;

varoperation: declaration | creation | definition | asignation;

block: SLbrack sentence* SRbrack | sentence;

funccall: funcresult SC;

funcresult: ID SLpar arguments? SRpar;

arguments: expression (Scomma expression)*;

declaration: Kvar asignation;

creation: Kset asignation;

definition: Kdef asignation;

asignation:
	ID Sequal expression					# simpleAssign
	| ID POTENCIALASSIGN expression			# potencialAssign
	| ID MULTIPLICATIVEASSIGN expression	# multiplicativeAssign
	| ID MODULEASSIGN expression			# moduleAssign
	| ID ADITIVEASSIGN expression			# aditiveAssign;

expression:
	SLpar expression SRpar								# parenthesisExp
	| <assoc = right> expression UNIARIT				# uniAritExp
	| <assoc = right> Snot expression					# uniNotExp
	| <assoc = right> expression POTENCIAL expression	# potencialExp
	| expression MULTIPLICATIVE expression				# multiplicativeExp
	| expression Smod expression						# moduleExp
	| expression ADITIVE expression						# aditiveExp
	| expression RELATIONAL expression					# relationalExp
	| expression EQRELATIONAL expression				# eqrelationalExp
	| expression EQUALITY expression					# equalityExp
	| expression ANDOR expression						# logicalExp
	| funcresult										# funcExp
	| value												# valueExp;

value: NULL | STRING | NUMBER | BOOL | GID | RID | ID;

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