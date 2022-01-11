using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Antlr4.Runtime;
using KYLib.Modding;
using KYLib.System;
using KYLib.Utils;
using Kys.Lang;
using Kys.Library;
using Kys.Parser;
using Kys.Visitors;

namespace Kys
{
	class Program
	{
		public static Dictionary<string, dynamic> Variables = new();

		public static Dictionary<string, IFunction> Functions = new();

		public static int ExitCode = 0;

		public static int LastLine = 0;

		static int Main(string[] args)
		{
			Mod.EnableAutoLoads();
			//IContext.ChangeContext(ContextFactory.Create(ContextFactoryType.ME));

			var Int = typeof(int);
			var Double = typeof(double);
			var a = Int.IsAssignableFrom(Double);
			var b = Double.IsAssignableFrom(Int);
			var c = Int.IsAssignableTo(Double);
			var d = Double.IsAssignableTo(Int);

			//IContext.Me.AddFunctions(typeof(Program));
			//IContext.Me.AddStandardFunctions();
			var func =
@"kyl gen ""Kys.Program, Kys""";
			var chars = CharStreams.fromString(func);
			var lexer = new KysLexer(chars);
			var tokens = new CommonTokenStream(lexer);
			var parser = new KysParser(tokens);
			parser.AddErrorListener(new Errores());
			var funcdef = parser.kyl();

			//KylLoader.Load(funcdef);

//			IFunction f = IContext.Me.GetFunction("Print");
			
		//	f.Call(null,null,(byte) 34 , 23, 46 , 45, 5);

	//		IContext.Start();
		//	IContext.Stop();
			return 0;
		}

		public static void Print(double obj) => Console.WriteLine(obj);

		public static void Print(double obj , string a) => Console.WriteLine(obj);

		public static void Print(double obj, string a, int b, int c) => Console.WriteLine(obj);


		private static async Task<string> waitfor(params int[] time)
		{
			foreach (var item in time)
			{
				await Task.Delay(item);
			}
			Console.WriteLine("waited");
			return time.ToString();
		}

		public delegate Task<string> asinc(int[] time);
	}

	class Errores : BaseErrorListener
	{
		public override void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
		{
			Environment.Exit(-1);
		}
	}
}
