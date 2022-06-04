@echo off
cd src\Kys.Parser

java -jar ..\..\build-tools\antlr4.jar -Dlanguage=CSharp KysParser.g4 KysLexer.g4 -no-listener -visitor -package Kys.Parser

dotnet run --project ..\..\tool\ParserEditor\ParserEditor.csproj -c Release KysParser.cs