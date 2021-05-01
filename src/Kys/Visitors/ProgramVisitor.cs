using System;
using Antlr4.Runtime.Misc;

namespace Kys.Visitors
{
	public class ProgramVisitor : KysParserBaseVisitor<int>
	{

		public override int VisitProgram([NotNull] KysParser.ProgramContext context)
		{
			return 0;
		}
	}
}