using Antlr4.Runtime.Misc;

namespace Kys.Interpreter.Visitors
{
	/// <summary>
	/// Implementación por defecto de <see cref="IVisitor{T}"/> para ejecutar <see cref="ProgramContext"/>.
	/// </summary>
	public class ProgramVisitor : BaseVisitor<dynamic>
	{
		IKysParserVisitor<object> IntructionVisitor;
		IKysParserVisitor<object> TopLevelVisitor;

		/// <inheritdoc/>
		public override void Configure(IServiceProvider serviceProvider)
		{
			base.Configure(serviceProvider);
			IntructionVisitor = VisitorProvider.GetVisitor<InstructionContext>();
			TopLevelVisitor = VisitorProvider.GetVisitor<ToplevelContext>();
		}

		/// <summary>
		/// Primero ejecuta todas los toplevel de <see cref="ProgramContext.toplevel()"/> y luego ejecuta las instrucciones de <see cref="ProgramContext.instruction()"/>.
		/// </summary>
		/// <inheritdoc/>
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
