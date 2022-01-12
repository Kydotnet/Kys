using Antlr4.Runtime.Misc;

namespace Kys.Interpreter.Visitors
{
	public class InstructionVisitor : KysParserBaseVisitor<object>
	{
		IInterpreterSesion Sesion;
		IKysParserVisitor<object> sentenceVisitor;
		IKysParserVisitor<dynamic> valueVisitor;

		public InstructionVisitor(IInterpreterSesion sesion, IVisitorProvider visitorProvider)
		{
			Sesion = sesion;
			sentenceVisitor = visitorProvider.GetVisitor<SentenceContext>();
			valueVisitor = visitorProvider.GetVisitor<ValueContext>();
		}

		public override object VisitInstruction([NotNull] InstructionContext context)
		{
			Sesion.LastLine = context.Start.Line;
			return base.VisitInstruction(context);
		}

		public override object VisitSentence([NotNull] SentenceContext context) =>
			sentenceVisitor.VisitSentence(context);

		public override object VisitExitprogram([NotNull] ExitprogramContext context)
		{
			double code = ValueVisitor.GetNumber(context.NUMBER());
			int exit = (int)Math.Round(code);
			Environment.ExitCode = exit;
			return false;
		}
	}
}