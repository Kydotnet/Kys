using Kys.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using static Kys.Parser.KysParser;

namespace Kys.Lang
{
	public sealed class KysFunction : IFunction
	{

		public IEnumerable<SentenceContext> Sentences { get; init; }

		public string Name { get; init; }

		public int ArgCount { get; init; }

		public bool InfArgs { get; init; }

		public IContext ParentContext { get; init; }

		public string[] ParamsNames { get; init; }

		/// <summary>
		/// Este es el Visitor que se usa para ejecutar las sentencias dentro de la función
		/// </summary>
		public static KysParserBaseVisitor<bool> Executor { get; set; } = new();

		public dynamic Call(IContext CallerContext, IScope FunctionScope, params dynamic[] args)
		{
			ValidateParams(args.Length);

			var Zip = Enumerable.Zip(ParamsNames, args);

			FunctionScope.Start();

			foreach (var item in Zip)
				FunctionScope.SetVar(item.First, item.Second);

			foreach (SentenceContext sentence in Sentences)
				Executor.VisitSentence(sentence);

			FunctionScope.DefVar("return", null, false);
			var ret = FunctionScope.GetVar("return", false);

			FunctionScope.Stop();

			return ret;
		}

		private void ValidateParams(int length)
		{

		}
	}
}