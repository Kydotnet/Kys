using System;
using Antlr4.Runtime.Misc;
using Kys.Exceptions;
using Kys.Parser;

namespace Kys.Visitors
{
	public class KysRunner : KysParserBaseVisitor<int>
	{

		public override int VisitProgram([NotNull] KysParser.ProgramContext context)
		{
			InstructionAnalizer analizer = new();
			try
			{
				// ejecutamos cada instrucción del programa.
				foreach (var item in context.instruction())
					if (!analizer.Analize(item)) break;
				return Program.ExitCode;
			}
			catch (KysException e)
			{
				Console.WriteLine("{0} {1}", e.Line, e.Message);

				return 1;
			}
			catch (Exception e)
			{
				Console.WriteLine("line {0}: {1}", Program.LastLine, e.Message);
				return 1;
			}

		}
	}
}