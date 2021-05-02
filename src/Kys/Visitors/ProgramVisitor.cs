using System;
using Antlr4.Runtime.Misc;
using Kys.Exceptions;

namespace Kys.Visitors
{
	public class ProgramVisitor : KysParserBaseVisitor<int>
	{

		public override int VisitProgram([NotNull] KysParser.ProgramContext context)
		{
			SentenceExecutor executor = new();
			try
			{
				// ejecutamos cada sentencia del programa.
				foreach (var item in context.sentence())
					executor.Visit(item);
				return 0;
			}
			catch (KysException e)
			{
				Console.WriteLine("{0} {1}", e.Line, e.Message);

				return 1;
			}
			catch (Exception)
			{
				throw;
			}

		}
	}
}