using Antlr4.Runtime.Misc;

namespace Kys.Interpreter.Visitors
{
	public class InstructionVisitor : BaseVisitor<object>
	{
		IKysParserVisitor<object> sentenceVisitor;

		public override void Configure(IServiceProvider serviceProvider)
		{
			base.Configure(serviceProvider);
			sentenceVisitor = VisitorProvider.GetVisitor<SentenceContext>();
		}


		public override object VisitInstruction([NotNull] InstructionContext context)
		{
			Sesion["LastLine"] = context.Start.Line;
			Sesion["LastInstruction"] = context;
			return base.VisitInstruction(context);
		}

		public override object VisitSentence([NotNull] SentenceContext context) =>
			sentenceVisitor.VisitSentence(context);

		public override object VisitFuncdefinition([NotNull] FuncdefinitionContext context)
		{
			Sesion.CurrentContext.AddKysFunction(context, sentenceVisitor);
			return null;
		}

		public override object VisitExitprogram([NotNull] ExitprogramContext context)
		{
			double code = ValueVisitor.GetNumber(context.NUMBER());
			int exit = (int)Math.Round(code);
			Environment.ExitCode = exit;
			return false;
		}
	}
}