#!/bin/bash

antlr4 -Dlanguage=CSharp KysLexer.g4 -no-listener -visitor -package Kys -o Generated
