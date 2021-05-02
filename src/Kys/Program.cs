using System;
using System.Collections.Generic;
using System.IO;
using Antlr4.Runtime;
using Kys.Visitors;

namespace Kys
{
	class Program
	{
		public static Dictionary<string, dynamic> Variables = new();

		static int Main(string[] args)
		{
			if (args.Length == 0)
				return 1;
			var path = args[0];

			using var input = File.OpenRead(path);
			ICharStream inputStream = CharStreams.fromStream(input);
			KysLexer kysLexer = new(inputStream);
			CommonTokenStream commonTokenStream = new(kysLexer);
			KysParser kysParser = new(commonTokenStream);
			// syntax check
			KysParser.ProgramContext programContext = kysParser.program();

			if (kysParser.NumberOfSyntaxErrors > 0)
				return 1;

			ProgramVisitor visitor = new();

			//execute the app
			var exit = visitor.Visit(programContext);

			return exit;
		}
	}
}
