using Antlr4.Runtime.Misc;
using Microsoft.CSharp.RuntimeBinder;

namespace Kys.Interpreter.Visitors
{
	public class ExpressionVisitor : BaseVisitor<dynamic>
	{
		IKysParserVisitor<dynamic> valueVisitor;
		IKysParserVisitor<dynamic> funcresultVisitor;

		public override void Configure(IServiceProvider serviceProvider)
		{
			base.Configure(serviceProvider);
			valueVisitor = VisitorProvider.GetVisitor<ValueContext>();
			funcresultVisitor = VisitorProvider.GetVisitor<FuncresultContext>();
		}

		public override dynamic VisitValueExp([NotNull] ValueExpContext context) =>
			valueVisitor.VisitValue(context.value());

		public override dynamic VisitParenthesisExp([NotNull] ParenthesisExpContext context) =>
			Visit(context.expression());

		public override dynamic VisitUniNotExp([NotNull] UniNotExpContext context)
		{
			dynamic val = Visit(context.expression());

			try
			{
				return !val;
			}
			catch (RuntimeBinderException e)
			{
				//throw new TokenException(context.Start, e.Message);
				throw e;
			}
		}

		public override dynamic VisitUniAritExp([NotNull] UniAritExpContext context)
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
				//throw new TokenException(context.Start, e.Message);
				throw e;
			}
		}

		public override dynamic VisitPotencialExp([NotNull] PotencialExpContext context)
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
				//throw new TokenException(context.Start, e.Message);
				throw e;
			}
		}

		public override dynamic VisitMultiplicativeExp([NotNull] MultiplicativeExpContext context)
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
				//throw new TokenException(context.Start, e.Message);
				throw e;
			}
		}

		public override dynamic VisitModuleExp([NotNull] ModuleExpContext context)
		{
			dynamic a = Visit(context.expression(0));
			dynamic b = Visit(context.expression(1));
			try
			{
				return a % b;
			}
			catch (RuntimeBinderException e)
			{
				//throw new TokenException(context.Start, e.Message);
				throw e;
			}
		}

		public override dynamic VisitAditiveExp([NotNull] AditiveExpContext context)
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
				//throw new TokenException(context.Start, e.Message);
				throw e;
			}
		}

		public override dynamic VisitEqualityExp([NotNull] EqualityExpContext context)
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
				//throw new TokenException(context.Start, e.Message);
				throw e;
			}
		}

		public override dynamic VisitLogicalExp([NotNull] LogicalExpContext context)
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
				//throw new TokenException(context.Start, e.Message);
				throw e;
			}
		}

		public override dynamic VisitEqrelationalExp([NotNull] EqrelationalExpContext context)
		{
			dynamic a = Visit(context.expression(0));
			dynamic b = Visit(context.expression(1));
			try
			{
				if (context.EQRELATIONAL().GetText() == "<=")
					return a <= b;
				return a >= b;
			}
			catch (RuntimeBinderException e)
			{
				//throw new TokenException(context.Start, e.Message);
				throw e;
			}
		}

		public override dynamic VisitRelationalExp([NotNull] RelationalExpContext context)
		{
			dynamic a = Visit(context.expression(0));
			dynamic b = Visit(context.expression(1));
			try
			{
				if (context.RELATIONAL().GetText() == "<")
					return a < b;
				return a > b;
			}
			catch (RuntimeBinderException e)
			{
				//throw new TokenException(context.Start, e.Message);
				throw e;
			}
		}

		public override dynamic VisitFuncExp([NotNull] FuncExpContext context)
		{
			return funcresultVisitor.VisitFuncresult(context.funcresult());
		}
	}
}