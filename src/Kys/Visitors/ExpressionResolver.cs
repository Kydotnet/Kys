using System;
using System.Dynamic;
using Antlr4.Runtime.Misc;
using Kys.Exceptions;
using Microsoft.CSharp.RuntimeBinder;

namespace Kys.Visitors
{
	internal class ExpressionResolver : KysParserBaseVisitor<dynamic>
	{
		public override dynamic VisitValueExp([NotNull] KysParser.ValueExpContext context) =>
			ValueResolver.Default.Visit(context);

		public override dynamic VisitParenthesisExp([NotNull] KysParser.ParenthesisExpContext context) =>
			Visit(context.expression());

		public override dynamic VisitUniNotExp([NotNull] KysParser.UniNotExpContext context)
		{
			dynamic val = Visit(context.expression());

			try
			{
				return !val;
			}
			catch (RuntimeBinderException e)
			{
				throw new TokenException(context.Start, e.Message);
			}
		}

		public override dynamic VisitUniAritExp([NotNull] KysParser.UniAritExpContext context)
		{
			dynamic val = Visit(context.expression());
			try
			{
				if (context.UNIARIT().GetText() == "++")
					return ++val;
				return --val;
			}
			catch (RuntimeBinderException e)
			{
				throw new TokenException(context.Start, e.Message);
			}
		}

		public override dynamic VisitPotencialExp([NotNull] KysParser.PotencialExpContext context)
		{
			dynamic a = Visit(context.expression(0));
			dynamic b = Visit(context.expression(1));
			try
			{
				if (context.POTENCIAL().GetText() == "^")
					return Math.Pow(a, b);
				return Math.Pow(b, 1d / a);
			}
			catch (RuntimeBinderException e)
			{
				throw new TokenException(context.Start, e.Message);
			}
		}

		public override dynamic VisitMultiplicativeExp([NotNull] KysParser.MultiplicativeExpContext context)
		{
			dynamic a = Visit(context.expression(0));
			dynamic b = Visit(context.expression(1));
			try
			{
				if (context.MULTIPLICATIVE().GetText() == "*")
					return a * b;
				return a / b;
			}
			catch (RuntimeBinderException e)
			{
				throw new TokenException(context.Start, e.Message);
			}
		}

		public override dynamic VisitAditiveExp([NotNull] KysParser.AditiveExpContext context)
		{
			dynamic a = Visit(context.expression(0));
			dynamic b = Visit(context.expression(1));
			try
			{
				if (context.ADITIVE().GetText() == "+")
					return a + b;
				return a - b;
			}
			catch (RuntimeBinderException e)
			{
				throw new TokenException(context.Start, e.Message);
			}
		}

		public override dynamic VisitEqualityExp([NotNull] KysParser.EqualityExpContext context)
		{
			dynamic a = Visit(context.expression(0));
			dynamic b = Visit(context.expression(1));
			try
			{
				if (context.EQUALITY().GetText() == "==")
					return a.Equals(b);
				return !a.Equals(b);
			}
			catch (RuntimeBinderException e)
			{
				throw new TokenException(context.Start, e.Message);
			}
		}

		public override dynamic VisitLogicalExp([NotNull] KysParser.LogicalExpContext context)
		{
			dynamic a = Visit(context.expression(0));
			dynamic b = Visit(context.expression(1));
			try
			{
				if (context.ANDOR().GetText() == "&&")
					return a && b;
				return a || b;
			}
			catch (RuntimeBinderException e)
			{
				throw new TokenException(context.Start, e.Message);
			}
		}
	}
}