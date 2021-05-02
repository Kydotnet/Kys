using Antlr4.Runtime.Misc;
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
			return Visit(context.asignation());
		}

		public override bool VisitAsignation([NotNull] KysParser.AsignationContext context)
		{
			var varname = context.VAR().GetText();
			if (!Program.Variables.ContainsKey(varname))
				throw new UndefinedException(context.VAR().Symbol, varname);
			ValueResolver resolver = new();

			Program.Variables[varname] = resolver.Visit(context.value());
			return true;
		}
	}
}