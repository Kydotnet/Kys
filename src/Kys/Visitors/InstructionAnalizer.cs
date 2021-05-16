using Kys.Parser;
using System;
using Antlr4.Runtime.Misc;

namespace Kys.Visitors
{
	public class InstructionAnalizer : KysParserBaseVisitor<bool>
	{
		public bool Analize([NotNull] KysParser.InstructionContext context) =>
			VisitInstruction(context);

		public override bool VisitInstruction([NotNull] KysParser.InstructionContext context)
		{
			Program.LastLine = context.Start.Line;
			return base.VisitInstruction(context);
		}

		public override bool VisitSentence([NotNull] KysParser.SentenceContext context) =>
			SentenceExecutor.Default.Visit(context);

		public override bool VisitExitprogram([NotNull] KysParser.ExitprogramContext context)
		{
			double code = ValueResolver.GetNumber(context.NUMBER());
			int exit = (int)Math.Round(code);
			Program.ExitCode = exit;
			return false;
		}
	}
}