using System;
using Antlr4.Runtime.Misc;

namespace Kys.Visitors
{
	public class ProgramVisitor : KysParserBaseVisitor<int>
	{

		public override int VisitProgram([NotNull] KysParser.ProgramContext context)
		{
			SentenceExecutor executor = new();

			// ejecutamos cada sentencia del programa.
			foreach (var item in context.sentence())
				executor.Visit(item);

			return 0;
		}
	}
}