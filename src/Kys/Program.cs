using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Antlr4.Runtime;
using Kys.Core;
using Kys.Visitors;

namespace Kys
{
	class Program
	{
		public static Dictionary<string, dynamic> Variables = new();

		public static Dictionary<string, Function> Functions = new();

		public static int ExitCode = 0;

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

			Config.ConfigFuncs();

			ProgramVisitor visitor = new();

			//execute the app
			ExitCode = visitor.Visit(programContext);

			PrintBeforExit();

			return ExitCode;
		}

		private static void PrintBeforExit()
		{
			Console.WriteLine("All defined variables:");
			foreach (var item in Variables)
				Console.WriteLine("{0}: {1}", item.Key, item.Value);
		}
	}
}
