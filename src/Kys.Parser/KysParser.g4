parser grammar KysParser;

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