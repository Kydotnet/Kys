using System;
using System.Linq;
using Antlr4.Runtime.Misc;
using Kys.Core;
using Kys.Exceptions;

namespace Kys.Visitors
{
	public class SentenceExecutor : KysParserBaseVisitor<bool>
	{
		public override bool VisitDeclaration([NotNull] KysParser.DeclarationContext context)
		{
			var varname = context.asignation().VAR().GetText();
			if (Program.Variables.ContainsKey(varname))
				throw new DefinedException(context.asignation().VAR().Symbol, varname);
			Program.Variables.Add(varname, null);
			try
			{
				return Visit(context.asignation());
			}
			catch (TokenException)
			{
				Program.Variables.Remove(varname);
				throw;
			}
		}

		public override bool VisitFunccall([NotNull] KysParser.FunccallContext context)
		{
			var funcname = context.funcresult().funcname().GetText();
			if (!Program.Functions.ContainsKey(funcname))
				return false;

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
			var varname = context.VAR().GetText();
			if (!Program.Variables.ContainsKey(varname))
				throw new UndefinedException(context.VAR().Symbol, varname);
			ExpressionResolver resolver = new();

			Program.Variables[varname] = resolver.Visit(context.expression());
			return true;
		}

		public override bool VisitExitprogram([NotNull] KysParser.ExitprogramContext context)
		{
			double code = ValueResolver.GetNumber(context.NUMBER());
			int exit = (int)Math.Round(code);
			Program.ExitCode = exit;
			return false;
		}
	}
}