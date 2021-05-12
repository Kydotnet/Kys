using System;
using System.Linq;
using Antlr4.Runtime.Misc;
using Kys.Core;
using Kys.Exceptions;

namespace Kys.Visitors
{
	public class SentenceExecutor : KysParserBaseVisitor<bool>
	{
		public static readonly SentenceExecutor Default = new();

		public override bool VisitControl([NotNull] KysParser.ControlContext context) =>
			ControlStructure.Default.Visit(context);

		public override bool VisitDeclaration([NotNull] KysParser.DeclarationContext context)
		{
			var varname = context.asignation().ID().GetText();
			if (Program.Variables.ContainsKey(varname))
				throw new DefinedException(context.asignation().ID().Symbol, varname);
			Program.Variables.Add(varname, null);
			try
			{
				return Visit(context.asignation());
			}
			catch (Exception)
			{
				Program.Variables.Remove(varname);
				throw;
			}
		}

		public override bool VisitFunccall([NotNull] KysParser.FunccallContext context)
		{
			var funcname = context.funcresult().ID().GetText();
			var nametoken = context.funcresult().ID().Symbol;

			if (!Program.Functions.ContainsKey(funcname))
				throw new UndefinedFunctionException(nametoken, funcname);

			var funcargs = context.funcresult().arguments();
			var hasargs = funcargs != null;

			dynamic[] args;
			if (hasargs)
			{
				args = funcargs.value().Select(v => ValueResolver.Default.Visit(v)).ToArray();
			}
			else
			{
				args = Array.Empty<dynamic>();
			}
			Function func = Program.Functions[funcname];
			_ = func.Call(args);
			return true;
		}

		public override bool VisitAsignation([NotNull] KysParser.AsignationContext context)
		{
			var varname = context.ID().GetText();
			if (!Program.Variables.ContainsKey(varname))
				throw new UndefinedException(context.ID().Symbol, varname);
			ExpressionResolver resolver = new();

			Program.Variables[varname] = resolver.Visit(context.expression());
			return true;
		}
	}
}