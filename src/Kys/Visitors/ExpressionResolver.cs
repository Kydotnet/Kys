using System;
using System.Dynamic;
using Antlr4.Runtime.Misc;
using Kys.Exceptions;

namespace Kys.Visitors
{
	internal class ExpressionResolver : KysParserBaseVisitor<dynamic>
	{
		public override dynamic VisitValueExp([NotNull] KysParser.ValueExpContext context) =>
			ValueResolver.Default.Visit(context);

		public override dynamic VisitParenthesisExp([NotNull] KysParser.ParenthesisExpContext context) =>
			Visit(context.expression());

		public override dynamic VisitBooleanExp([NotNull] KysParser.BooleanExpContext context)
		{
			dynamic a = Visit(context.expression(0));
			dynamic b = Visit(context.expression(1));
			try
			{
				if (context.ANDOR().GetText() == "&&")
					return a && b;
				return a || b;
			}
			catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException e)
			{
				throw new TokenException(context.Start, e.Message);
			}
		}
	}
}