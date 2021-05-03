@echo off
antlr4 -Dlanguage=CSharp KysParser.g4 KysLexer.g4 -no-listener -visitor -package Kys -o Generated