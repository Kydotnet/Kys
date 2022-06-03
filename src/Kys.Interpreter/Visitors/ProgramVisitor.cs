using Antlr4.Runtime.Misc;
using Kys.Runtime;

namespace Kys.Interpreter.Visitors
{
	/// <summary>
	/// Implementación por defecto de <see cref="IVisitor{T}"/> para ejecutar <see cref="ProgramContext"/>.
	/// </summary>
	public class ProgramVisitor : BaseVisitor<IKyObject>
	{
		#pragma warning disable CS8618
		IKysParserVisitor<IKyObject> _intructionVisitor;
		IKysParserVisitor<IKyObject> _topLevelVisitor;
		#pragma warning restore CS8618

		/// <inheritdoc/>
		public override void Configure(IServiceProvider serviceProvider)
		{
			base.Configure(serviceProvider);
			_intructionVisitor = VisitorProvider.GetVisitor<InstructionContext>();
			_topLevelVisitor = VisitorProvider.GetVisitor<ToplevelContext>();
		}

		/// <summary>
		/// Primero ejecuta todas los toplevel de <see cref="ProgramContext.toplevel()"/> y luego ejecuta las instrucciones de <see cref="ProgramContext.instruction()"/>.
		/// </summary>
		/// <inheritdoc/>
		public override IKyObject VisitProgram([NotNull] ProgramContext context)
		{
			var toplevel = context.toplevel();
			var instructions = context.instruction();
			
			
			foreach (var top in toplevel)
				_topLevelVisitor.VisitToplevel(top);

			var skips = 0;
			foreach (var item in instructions)
			{
				if (skips > 0)
				{
					skips--;
					continue;
				}
				var ret = _intructionVisitor.VisitInstruction(item);
				if (ret == False)
					break;
				if (ret is KyObject<int> skip)
					skips = skip;
			}

			return False;
		}
	}
}
