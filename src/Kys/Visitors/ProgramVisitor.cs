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
					if (!executor.Visit(item)) break;
				return Program.ExitCode;
			}
			catch (KysException e)
			{
				Console.WriteLine("{0} {1}", e.Line, e.Message);

				return 1;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return 1;
			}

		}
	}
}