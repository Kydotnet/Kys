using Antlr4.Runtime.Misc;

namespace Kys.Interpreter.Visitors
{
	public class ProgramVisitor : BaseVisitor<dynamic>
	{
		IKysParserVisitor<object> IntructionVisitor;
		IKysParserVisitor<object> TopLevelVisitor;

		public override void Configure(IServiceProvider serviceProvider)
		{
			base.Configure(serviceProvider);
			IntructionVisitor = VisitorProvider.GetVisitor<InstructionContext>();
			TopLevelVisitor = VisitorProvider.GetVisitor<ToplevelContext>();
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
