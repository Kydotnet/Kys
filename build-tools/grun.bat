@echo off

java -jar antlr4.jar Kys.g4

javac -cp ".;antlr4.jar" *.java

java -cp ".;antlr4.jar" org.antlr.v4.gui.TestRig Kys program -gui %1