using Kys.Parser;
using Antlr4.Runtime.Misc;

namespace Kys.Visitors
{
	public class ControlStructure : KysParserBaseVisitor<bool>
	{
		public static readonly ControlStructure Default = new();

		public override bool VisitIfcontrol([NotNull] KysParser.IfcontrolContext context)
		{
			dynamic exp = ExpressionResolver.Default.Visit(context.expression());
			if (exp)
				Visit(context.block());
			else if (context.elsecontrol() != null)
				Visit(context.elsecontrol());
			return true;
		}

		public override bool VisitElsecontrol([NotNull] KysParser.ElsecontrolContext context)
		{
			if (context.ifcontrol() != null)
				Visit(context.ifcontrol());
			else
				Visit(context.block());
			return true;
		}

		public override bool VisitWhilecontrol([NotNull] KysParser.WhilecontrolContext context)
		{
			var block = context.block();

			while (ExpressionResolver.Default.Visit(context.expression()))
				Visit(block);

			return true;
		}

		public override bool VisitBlock([NotNull] KysParser.BlockContext context)
		{
			foreach (var item in context.sentence())
				SentenceExecutor.Default.Visit(item);
			return true;
		}
	}
}