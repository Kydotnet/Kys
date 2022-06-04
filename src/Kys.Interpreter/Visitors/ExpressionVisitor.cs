using Antlr4.Runtime.Misc;
using Kys.Parser.Extensions;
using Kys.Runtime;
using Microsoft.CSharp.RuntimeBinder;
// TODO: rethrow exceptions with KyTypes
// ReSharper disable PossibleIntendedRethrow
#pragma warning disable CS8764

namespace Kys.Interpreter.Visitors;

/// <summary>
/// Implementación por defecto de <see cref="IVisitor{T}"/> para ejecutar <see cref="ExpressionContext"/>.
/// </summary>
public class ExpressionVisitor : BaseVisitor<IKyObject>
{
#pragma warning disable CS8618
	IKysParserVisitor<IKyObject> _valueVisitor;
	IKysParserVisitor<IKyObject> _funcresultVisitor;
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
	public override IKyObject VisitValueExp([NotNull] ValueExpContext context) =>
		_valueVisitor.VisitValue(context.Value);

	/// <summary>
	/// Evalua la expresión interna, es decir, la que se encuentra dentro de los parentesis.
	/// </summary>
	/// <inheritdoc/>
	public override IKyObject VisitParenthesisExp([NotNull] ParenthesisExpContext context) =>
		Visit(context.Expression);

	/// <summary>
	/// Evalua la exrpesión <paramref name="context"/> con el operador de C# "<c>!</c>".
	/// </summary>
	/// <inheritdoc/>
	public override IKyObject VisitUniNotExp([NotNull] UniNotExpContext context)
	{
		var val = Visit(context.Expression);
		if (val.CanOperate(OperatorType.UnitaryNegation))
			return val.UnaryNegation();
		throw new NotImplementedException();
	}

	/// <summary>
	/// Evalua la exrpesión <paramref name="context"/> con los operadores de C# "pre <c>++</c>" y "pre <c>--</c>" dependiendo de <see cref="UniAritExpContext.UNIARIT()"/>.
	/// </summary>
	/// <inheritdoc/>
	public override IKyObject VisitUniAritExp([NotNull] UniAritExpContext context)
	{

		if (context.Expression is ValueExpContext valueexp && valueexp.Value.ID != null)
			return VisitUniAritValueExp(valueexp, context.UNIARITText == "++");

		var val = Visit(context.Expression);

		if(val.CanOperate(OperatorType.UnitaryOperation))
			return val.UnitaryOperation(context.UNIARITText == "++");

		throw new NotImplementedException();
	}

	IKyObject VisitUniAritValueExp(ValueExpContext valueExpContext, bool v)
	{
		var name = valueExpContext.Value.IDText;
		var val = Sesion.CurrentScope.GetVar(name);
		if (val.CanOperate(OperatorType.UnitaryOperation))
			Sesion.CurrentScope.AsigVar(name, val.UnitaryOperation(v));
		else throw new NotImplementedException();
		return val;
	}

	/// <summary>
	/// Evalua la exrpesión <paramref name="context"/> con el metodo <see cref="Math.Pow(double, double)"/>
	/// </summary>
	/// <inheritdoc/>
	public override IKyObject VisitPotencialExp([NotNull] PotencialExpContext context)
	{
		var a = Visit(context.expression(0));
		var b = Visit(context.expression(1));
		var pot = context.POTENCIALText == "^";

		if (a.CanOperate(OperatorType.Potencial))
			return a.Potencial(pot, b);
		else if(b.CanOperate(OperatorType.Potencial))
			return b.Potencial(pot, a);

		throw new NotImplementedException();
	}

	/// <summary>
	/// Evalua la exrpesión <paramref name="context"/> con los operadores de C# "<c>*</c>" y "<c>/</c>" dependiendo de <see cref="MultiplicativeExpContext.MULTIPLICATIVE()"/>.
	/// </summary>
	/// <inheritdoc/>
	public override IKyObject VisitMultiplicativeExp([NotNull] MultiplicativeExpContext context)
	{
		var a = Visit(context.expression(0));
		var b = Visit(context.expression(1));
		var pot = context.MULTIPLICATIVEText == "*";

		if (a.CanOperate(OperatorType.Multiplicative))
			return a.Multiplicative(pot, b);
		else if (b.CanOperate(OperatorType.Multiplicative))
			return b.Multiplicative(pot, a);

		throw new NotImplementedException();
	}

	/// <summary>
	/// Evalua la exrpesión <paramref name="context"/> con el operador de C# "<c>%</c>".
	/// </summary>
	/// <inheritdoc/>
	public override IKyObject VisitModuleExp([NotNull] ModuleExpContext context)
	{
		var a = Visit(context.expression(0));
		var b = Visit(context.expression(1));

		if (a.CanOperate(OperatorType.Module))
			return a.Module(b);
		else if (b.CanOperate(OperatorType.Module))
			return b.Module(a);

		throw new NotImplementedException();
	}

	/// <summary>
	/// Evalua la exrpesión <paramref name="context"/> con los operadores de C# "<c>+</c>" y "<c>-</c>" dependiendo de <see cref="KysParser.AditiveExpContext.ADITIVE()"/>.
	/// </summary>
	/// <inheritdoc/>
	public override IKyObject VisitAditiveExp([NotNull] AditiveExpContext context)
	{
		var a = Visit(context.expression(0));
		var b = Visit(context.expression(1));
		var pot = context.Pot;

		if (a.CanOperate(OperatorType.Aditive))
			return a.Aditive(pot, b);
		else if (b.CanOperate(OperatorType.Aditive))
			return b.Aditive(pot, a);

		throw new NotImplementedException();
	}

	/// <summary>
	/// Evalua la exrpesión <paramref name="context"/> con el metodo <see cref="object.Equals(object)"/>.
	/// </summary>
	/// <inheritdoc/>
	public override IKyObject VisitEqualityExp([NotNull] EqualityExpContext context)
	{
		var a = Visit(context.expression(0));
		var b = Visit(context.expression(1));

		var pot = context.EQUALITYText == "==";

		if (a.CanOperate(OperatorType.Equality))
			return a.Equality(pot, b);
		else if (b.CanOperate(OperatorType.Equality))
			return b.Equality(pot, a);

		throw new NotImplementedException();
	}

	/// <summary>
	/// Evalua la expresión <paramref name="context"/> con los operadores de C# "<c>&amp;&amp;</c>" y "<c>||</c>" dependiendo de <see cref="LogicalExpContext.ANDOR()"/>.
	/// </summary>
	/// <inheritdoc/>
	public override IKyObject VisitLogicalExp([NotNull] LogicalExpContext context)
	{
		var a = Visit(context.expression(0));
		var b = Visit(context.expression(1));
		var pot = context.ANDORText == "&&";

		return a.CanOperate(OperatorType.Boolean) && b.CanOperate(OperatorType.Boolean)
			? pot ? a.Boolean() && b.Boolean() ? True : False : a.Boolean() || b.Boolean() ? True : False
			: throw new NotImplementedException();
	}

	/// <summary>
	/// Evalua la expresión <paramref name="context"/> con los operadores de C# "<c>&lt;=</c>" y "<c>&gt;=</c>" dependiendo de <see cref="EqrelationalExpContext.EQRELATIONAL()"/>.
	/// </summary>
	/// <inheritdoc/>
	public override IKyObject VisitEqrelationalExp([NotNull] EqrelationalExpContext context)
	{
		var a = Visit(context.expression(0));
		var b = Visit(context.expression(1));

		var pot = context.EQRELATIONALText == "<=";

		if (a.CanOperate(OperatorType.EqualityRelacional))
			return a.EqualityRelacional(pot, b);
		else if (b.CanOperate(OperatorType.EqualityRelacional))
			return b.EqualityRelacional(pot, a);

		throw new NotImplementedException();
	}

	/// <summary>
	/// Evalua la exrpesión <paramref name="context"/> con los operadores de C# "<c>&lt;</c>" y "<c>&gt;</c>" dependiendo de <see cref="RelationalExpContext.RELATIONAL()"/>.
	/// </summary>
	/// <inheritdoc/>
	public override IKyObject VisitRelationalExp([NotNull] RelationalExpContext context)
	{
		var a = Visit(context.Expression[0]);
		var b = Visit(context.Expression[1]);
		var pot = context.Pot;

		if (a.CanOperate(OperatorType.Relacional))
			return a.Relacional(pot, b);
		else if (b.CanOperate(OperatorType.Relacional))
			return b.Relacional(pot, a);

		throw new NotImplementedException();
	}

	/// <summary>
	/// Evalua el valor de <see cref="FuncExpContext.funcresult()"/> con el <see cref="IVisitor{T}"/> para el contexto <see cref="FuncresultContext"/>.
	/// </summary>
	/// <inheritdoc/>
	public override IKyObject VisitFuncExp([NotNull] FuncExpContext context)
	{
		Sesion["LastColumn"] = context.Start.Column;
		Sesion["LastLine"] = context.Start.Line;
		return _funcresultVisitor.VisitFuncresult(context.Funcresult);
	}
}
