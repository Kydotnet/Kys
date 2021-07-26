parser grammar KysParser;

options {
	tokenVocab = KysLexer;
}

program: (toplevel)* (instruction)+;

toplevel: kyl | kys;

kyl: Kkyl ID STRING;

kys: Kkys ID value*;

instruction: exitprogram | funcdefinition | sentence;

funcdefinition: Kfunc ID SLpar parameters SRpar block;

parameters: params? PARAMS?;

params: ID (Scomma ID)*;

exitprogram: Kexit NUMBER SC;

sentence: control | funccall | varoperation;

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

varoperation: declaration | creation | definition | asignation;

block: SLbrack sentence* SRbrack | sentence;

funccall: funcresult SC;

funcresult: ID SLpar arguments? SRpar;

arguments: expression (Scomma expression)*;

declaration: Kvar asignation;

creation: Kset asignation;

definition: Kdef asignation;

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

value: NULL | STRING | NUMBER | BOOL | ID;