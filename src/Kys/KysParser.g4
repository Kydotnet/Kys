parser grammar KysParser;

options {
	tokenVocab = KysLexer;
}

program: sentence+;

sentence: declaration | asignation;

declaration: Kvar asignation;

asignation: VAR Sequal value SC;

// un valor asignable puede ser otra variable o un numero o un string
value: STRING | NUMBER | VAR;