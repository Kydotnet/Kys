parser grammar KysParser;

options {
	tokenVocab = KysLexer;
}

program: sentence+;

sentence: exitprogram | funccall | declaration | asignation;

exitprogram: Kexit NUMBER SC;

declaration: Kvar asignation;

asignation: VAR Sequal expression SC;

expression:
	SLpar expression SRpar								# parenthesisExp
	| <assoc = right> Snot expression					# uniNotExp
	| <assoc = right> expression UNIARIT				# uniAritExp
	| <assoc = right> expression POTENCIAL expression	# potencialExp
	| expression MULTIPLICATIVE expression				# multiplicativeExp
	| expression ADITIVE expression						# aditiveExp
	| expression EQUALITY expression					# equalityExp
	| expression ANDOR expression						# logicalExp
	| value												# valueExp;
//| funcresult # funcExp

funccall: funcresult SC;

funcresult: funcname SLpar arguments? SRpar;

funcname: VAR | FUNC;

arguments: value (Scomma value)*;

value: STRING | NUMBER | BOOL | VAR;