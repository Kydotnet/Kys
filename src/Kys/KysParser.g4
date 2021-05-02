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
	SLpar expression SRpar			# parenthesisExp
	| expression ANDOR expression	# booleanExp
	| funcresult					# funcExp
	| value							# valueExp;

// un valor asignable puede ser otra variable o un numero o un string
value: STRING | NUMBER | BOOL | VAR;

funccall: funcresult SC;

funcresult: funcname SLpar arguments? SRpar;

funcname: VAR | FUNC;

arguments: value (Scomma value)*;