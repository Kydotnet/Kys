parser grammar KysParser;

options {
	tokenVocab = KysLexer;
}

program: sentence+;

sentence: declaration | asignation;

declaration: Kvar asignation;

asignation: VAR Sequal expression SC;

expression:
	SLpar expression SRpar			# parenthesisExp
	| expression ANDOR expression	# booleanExp
	| value							# valueExp;

// un valor asignable puede ser otra variable o un numero o un string
value: STRING | NUMBER | BOOL | VAR;