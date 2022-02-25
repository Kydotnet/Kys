using Antlr4.Runtime.Misc;
using Microsoft.CSharp.RuntimeBinder;
// TODO: rethrow exceptions with KyTypes
// ReSharper disable PossibleIntendedRethrow
#pragma warning disable CS8764

namespace Kys.Interpreter.Visitors;

/// <summary>
/// Implementación por defecto de <see cref="IVisitor{T}"/> para ejecutar <see cref="ExpressionContext"/>.
/// </summary>
public class ExpressionVisitor : BaseVisitor<dynamic>
{
#pragma warning disable CS8618
	IKysParserVisitor<dynamic> _valueVisitor;
	IKysParserVisitor<dynamic> _funcresultVisitor;
#pragma warning restore CS8618
	
	/// <inheritdoc/>
	public override void Configure(IServiceProvider serviceProvider)
	{
		base.Configure(serviceProvider);
		_valueVisitor = VisitorProvider.GetVisitor<ValueContext>();
		_funcresultVisitor = VisitorProvider.GetVisitor<FuncresultContext>();
	}

	/// <summary>
	/// Evalua el valor de <see cref="ValueExpContext.value()"/> con el <see cref="IVisitor{T}"/> para el contexto <see cref="ValueContext"/>.
	/// </summary>
	/// <inheritdoc/>
	public override dynamic VisitValueExp([NotNull] ValueExpContext context) =>
		_valueVisitor.VisitValue(context.value());

	/// <summary>
	/// Evalua la expresión interna, es decir, la que se encuentra dentro de los parentesis.
	/// </summary>
	/// <inheritdoc/>
	public override dynamic? VisitParenthesisExp([NotNull] ParenthesisExpContext context) =>
		Visit(context.expression());

	/// <summary>
	/// Evalua la exrpesión <paramref name="context"/> con el operador de C# "<c>!</c>".
	/// </summary>
	/// <inheritdoc/>
	public override dynamic? VisitUniNotExp([NotNull] UniNotExpContext context)
	{
		var val = Visit(context.expression());

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

	/// <summary>
	/// Evalua la exrpesión <paramref name="context"/> con los operadores de C# "pre <c>++</c>" y "pre <c>--</c>" dependiendo de <see cref="UniAritExpContext.UNIARIT()"/>.
	/// </summary>
	/// <inheritdoc/>
	public override dynamic VisitUniAritExp([NotNull] UniAritExpContext context)
	{
		if (context.expression() is ValueExpContext valueexp && valueexp.value().ID() != null)
			return VisitUniAritValueExp(valueexp);

		var val = Visit(context.expression());

		try
		{
			return (context.UNIARIT().GetText() == "++" ? ++val : --val)!;
		}
		catch (RuntimeBinderException e)
		{
			//throw new TokenException(context.Start, e.Message);
			throw e;
		}
	}

	dynamic VisitUniAritValueExp(ValueExpContext valueExpContext)
	{
		var name = valueExpContext.value().ID().GetText();
		var val = Sesion.CurrentScope.GetVar(name);
		Sesion.CurrentScope.AsigVar(name, ++val);
		return val!;
	}

	/// <summary>
	/// Evalua la exrpesión <paramref name="context"/> con el metodo <see cref="Math.Pow(double, double)"/>
	/// </summary>
	/// <inheritdoc/>
	public override dynamic VisitPotencialExp([NotNull] PotencialExpContext context)
	{
		var a = Visit(context.expression(0));
		var b = Visit(context.expression(1));
		try
		{
			return context.POTENCIAL().GetText() == "^" ? Math.Pow(a, b) : Math.Pow(b, 1d / a);
		}
		catch (RuntimeBinderException e)
		{
			//throw new TokenException(context.Start, e.Message);
			throw e;
		}
	}

	/// <summary>
	/// Evalua la exrpesión <paramref name="context"/> con los operadores de C# "<c>*</c>" y "<c>/</c>" dependiendo de <see cref="MultiplicativeExpContext.MULTIPLICATIVE()"/>.
	/// </summary>
	/// <inheritdoc/>
	public override dynamic VisitMultiplicativeExp([NotNull] MultiplicativeExpContext context)
	{
		var a = Visit(context.expression(0));
		var b = Visit(context.expression(1));
		try
		{
			return context.MULTIPLICATIVE().GetText() == "*" ? a * b : a / b;
		}
		catch (RuntimeBinderException e)
		{
			//throw new TokenException(context.Start, e.Message);
			throw e;
		}
	}

	/// <summary>
	/// Evalua la exrpesión <paramref name="context"/> con el operador de C# "<c>%</c>".
	/// </summary>
	/// <inheritdoc/>
	public override dynamic VisitModuleExp([NotNull] ModuleExpContext context)
	{
		var a = Visit(context.expression(0));
		var b = Visit(context.expression(1));
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

	/// <summary>
	/// Evalua la exrpesión <paramref name="context"/> con los operadores de C# "<c>+</c>" y "<c>-</c>" dependiendo de <see cref="AditiveExpContext.ADITIVE()"/>.
	/// </summary>
	/// <inheritdoc/>
	public override dynamic VisitAditiveExp([NotNull] AditiveExpContext context)
	{
		var a = Visit(context.expression(0));
		var b = Visit(context.expression(1));
		try
		{
			return context.ADITIVE().GetText() == "+" ? a + b : a - b;
		}
		catch (RuntimeBinderException e)
		{
			//throw new TokenException(context.Start, e.Message);
			throw e;
		}
	}

	/// <summary>
	/// Evalua la exrpesión <paramref name="context"/> con el metodo <see cref="object.Equals(object)"/>.
	/// </summary>
	/// <inheritdoc/>
	public override dynamic VisitEqualityExp([NotNull] EqualityExpContext context)
	{
		var a = Visit(context.expression(0));
		var b = Visit(context.expression(1));
		
		try
		{
			return context.EQUALITY().GetText() == "==" ? a == b : a != b;
		}
		catch (RuntimeBinderException e)
		{
			//throw new TokenException(context.Start, e.Message);
			throw e;
		}
	}

	/// <summary>
	/// Evalua la expresión <paramref name="context"/> con los operadores de C# "<c>&amp;&amp;</c>" y "<c>||</c>" dependiendo de <see cref="LogicalExpContext.ANDOR()"/>.
	/// </summary>
	/// <inheritdoc/>
	public override dynamic VisitLogicalExp([NotNull] LogicalExpContext context)
	{
		var a = Visit(context.expression(0));
		var b = Visit(context.expression(1));
		try
		{
			return context.ANDOR().GetText() == "&&" ? a && b : a || b;
		}
		catch (RuntimeBinderException e)
		{
			//throw new TokenException(context.Start, e.Message);
			throw e;
		}
	}

	/// <summary>
	/// Evalua la expresión <paramref name="context"/> con los operadores de C# "<c>&lt;=</c>" y "<c>&gt;=</c>" dependiendo de <see cref="EqrelationalExpContext.EQRELATIONAL()"/>.
	/// </summary>
	/// <inheritdoc/>
	public override dynamic VisitEqrelationalExp([NotNull] EqrelationalExpContext context)
	{
		var a = Visit(context.expression(0));
		var b = Visit(context.expression(1));
		try
		{
			return context.EQRELATIONAL().GetText() == "<=" ? a <= b : a >= b;
		}
		catch (RuntimeBinderException e)
		{
			//throw new TokenException(context.Start, e.Message);
			throw e;
		}
	}

	/// <summary>
	/// Evalua la exrpesión <paramref name="context"/> con los operadores de C# "<c>&lt;</c>" y "<c>&gt;</c>" dependiendo de <see cref="RelationalExpContext.RELATIONAL()"/>.
	/// </summary>
	/// <inheritdoc/>
	public override dynamic VisitRelationalExp([NotNull] RelationalExpContext context)
	{
		var a = Visit(context.expression(0));
		var b = Visit(context.expression(1));
		try
		{
			return context.RELATIONAL().GetText() == "<" ? a < b : a > b;
		}
		catch (RuntimeBinderException e)
		{
			//throw new TokenException(context.Start, e.Message);
			throw e;
		}
	}

	/// <summary>
	/// Evalua el valor de <see cref="FuncExpContext.funcresult()"/> con el <see cref="IVisitor{T}"/> para el contexto <see cref="FuncresultContext"/>.
	/// </summary>
	/// <inheritdoc/>
	public override dynamic VisitFuncExp([NotNull] FuncExpContext context)
	{
		Sesion["LastColumn"] = context.Start.Column;
		Sesion["LastLine"] = context.Start.Line;
		return _funcresultVisitor.VisitFuncresult(context.funcresult());
	}
}
