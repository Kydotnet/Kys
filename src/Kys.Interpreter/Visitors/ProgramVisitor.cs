using Antlr4.Runtime.Misc;

namespace Kys.Interpreter.Visitors
{
	public class ProgramVisitor : KysParserBaseVisitor<dynamic>
	{
		IKysParserVisitor<object> IntructionVisitor;
		IKysParserVisitor<object> TopLevelVisitor;

		public ProgramVisitor(IVisitorProvider visitorProvider)
		{
			IntructionVisitor = visitorProvider.GetVisitor<InstructionContext>();
			TopLevelVisitor = visitorProvider.GetVisitor<ToplevelContext>();
		}

		public override dynamic VisitProgram([NotNull] ProgramContext context)
		{
			var toplevel = context.toplevel();
			var instructions = context.instruction();

			foreach (var top in toplevel)
			{
				TopLevelVisitor.VisitToplevel(top);
			}

			foreach (var item in instructions)
			{
				if (IntructionVisitor.VisitInstruction(item) != null)
					break;
			}

			return null;
		}
	}
}
