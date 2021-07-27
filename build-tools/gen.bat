@echo off

java -jar antlr4.jar Kys.g4

javac -cp ".;antlr4.jar" *.java
