using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Kys.Parser.Extensions;
using Kys.Runtime;

namespace Kys.Interpreter.Visitors;

/// <summary>
/// Implementación por defecto de <see cref="IVisitor{T}"/> para ejecutar <see cref="VaroperationContext"/>.
/// </summary>
public class VaroperationVisitor : BaseVisitor<IKyObject>
{
	#pragma warning disable CS8618
	IKysParserVisitor<IKyObject> _expressionVisitor;
	#pragma warning restore CS8618

	/// <inheritdoc/>
	public override void Configure(IServiceProvider serviceProvider)
	{
		base.Configure(serviceProvider);

		_expressionVisitor = VisitorProvider.GetVisitor<ExpressionContext>();
	}

	/// <summary>
	/// Declara una variable usando <see cref="IScope.DecVar"/> del scope actual de la sesion.
	/// </summary>
	/// <inheritdoc/>
	public override IKyObject VisitDeclaration([NotNull] DeclarationContext context)
	{
		VisitVaroperation(context.Asignation, Sesion.CurrentScope.DecVar);
		return True;
	}

	/// <summary>
	/// Crea una variable usando <see cref="IScope.SetVar"/> del scope actual de la sesion.
	/// </summary>
	/// <inheritdoc/>
	public override IKyObject VisitCreation([NotNull] CreationContext context)
	{
		VisitVaroperation(context.Asignation, Sesion.CurrentScope.SetVar);
		return True;
	}

	void VisitVaroperation(AsignationContext context, Action<string, IKyObject, bool> actionVar)
	{
		var name = context.IDText;
		var valueExp = context.Expression;
		var val = _expressionVisitor.Visit(valueExp);

		actionVar(name, val, true);
	}

	/// <summary>
	/// Define una variable usando <see cref="IScope.DefVar"/> del scope actual de la sesion.
	/// </summary>
	/// <inheritdoc/>
	public override IKyObject VisitDefinition([NotNull] DefinitionContext context)
	{
		VisitVaroperation(context.Asignation, Sesion.CurrentScope.DefVar);

		return True;
	}

	/// <summary>
	/// Asigna el valor de <see cref="SimpleAssignContext.expression()"/> a la variable con nombre <see cref="SimpleAssignContext.ID()"/>
	/// </summary>
	/// <inheritdoc/>
	public override IKyObject VisitSimpleAssign([NotNull] SimpleAssignContext context)
	{
		VisitAsignation(context.IDText, context.Expression, context.SequalText, context.Sequal);
		return True;
	}

	void VisitAsignation(string id, IParseTree expressionContext, string operation, ITerminalNode simbol)
	{ 
		var value = _expressionVisitor.Visit(expressionContext);
		Sesion["LastToken"] = simbol;
		switch (operation)
		{
			case "=": break;
			case "+=":
				value = Sesion.CurrentScope.GetVar(id).Aditive(true, value);
				break;
			case "-=":
				value = Sesion.CurrentScope.GetVar(id).Aditive(false, value);
				break;
			case "*=":
				value = Sesion.CurrentScope.GetVar(id).Multiplicative(true, value);
				break;
			case "/=":
				value = Sesion.CurrentScope.GetVar(id).Multiplicative(false, value);
				break;
			case "%=":
				value = Sesion.CurrentScope.GetVar(id).Module(value);
				break;
			case "^=":
				value = Sesion.CurrentScope.GetVar(id).Potencial(true, value);
				break;
			case "~=":
				value = Sesion.CurrentScope.GetVar(id).Potencial(false, value);
				break;

		}
		Sesion.CurrentScope.AsigVar(id, value);
	}

	/// <summary>
	/// Calcula el valor de <see cref="PotencialAssignContext.expression()"/> elevado o como raiz con el valor que se encuentra en la variable <see cref="PotencialAssignContext.ID()"/> y lo asigna nuevamente a esa variable.
	/// </summary>
	/// <inheritdoc/>
	public override IKyObject VisitPotencialAssign([NotNull] PotencialAssignContext context)
	{
		VisitAsignation(context.IDText, context.Expression, context.POTENCIALASSIGNText, context.POTENCIALASSIGN);
		return True;
	}

	/// <summary>
	/// Calcula el valor de <see cref="MultiplicativeAssignContext.expression()"/> multiplicado o dividido en el valor que se encuentra en la variable <see cref="MultiplicativeAssignContext.ID()"/> y lo asigna nuevamente a esa variable.
	/// </summary>
	/// <inheritdoc/>
	public override IKyObject VisitMultiplicativeAssign([NotNull] MultiplicativeAssignContext context)
	{
		VisitAsignation(context.IDText, context.Expression, context.MULTIPLICATIVEASSIGNText, context.MULTIPLICATIVEASSIGN);
		return True;
	}

	/// <summary>
	/// Calcula el valor de la variable <see cref="ModuleAssignContext.ID()"/> modulo el valor de <see cref="ModuleAssignContext.expression()"/> y lo asigna nuevamente a esa variable.
	/// </summary>
	/// <inheritdoc/>
	public override IKyObject VisitModuleAssign([NotNull] ModuleAssignContext context)
	{
		VisitAsignation(context.IDText, context.Expression, context.MODULEASSIGNText, context.MODULEASSIGN);
		return True;
	}

	/// <summary>
	/// Calcula el valor de <see cref="AditiveAssignContext.expression()"/> sumado o restado en el valor que se encuentra en la variable <see cref="AditiveAssignContext.ID()"/> y lo asigna nuevamente a esa variable.
	/// </summary>
	/// <inheritdoc/>
	public override IKyObject VisitAditiveAssign([NotNull] AditiveAssignContext context)
	{
		VisitAsignation(context.IDText, context.Expression, context.ADITIVEASSIGNText, context.ADITIVEASSIGN);
		return True;
	}
}
