parser grammar KysParser;

options {
	tokenVocab = KysLexer;
}

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
	Kfor SLpar varoperation? SC expression? SC forexpression? SRpar block;

forexpression: expression | varoperation;

varoperation:
	declaration
	| creation
	| definition
	| selfasignation;

block: SLbrack sentence* SRbrack | sentence;

funccall: funcresult SC;

funcresult: ID SLpar arguments? SRpar;

arguments: expression (Scomma expression)*;

declaration: Kvar asignation;

creation: Kset asignation;

definition: Kdef asignation;

asignation: ID Sequal expression;

selfasignation:
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

value: NULL | STRING | NUMBER | BOOL | ID;