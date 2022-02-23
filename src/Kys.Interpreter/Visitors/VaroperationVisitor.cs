using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
namespace Kys.Interpreter.Visitors;

/// <summary>
/// Implementación por defecto de <see cref="IVisitor{T}"/> para ejecutar <see cref="VaroperationContext"/>.
/// </summary>
public class VaroperationVisitor : BaseVisitor<object>
{
	#pragma warning disable CS8618
	IKysParserVisitor<dynamic> _expressionVisitor;
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
	public override object VisitDeclaration([NotNull] DeclarationContext context)
	{
		VisitVaroperation(context.asignation(), Sesion.CurrentScope.DecVar);
		return true;
	}

	/// <summary>
	/// Crea una variable usando <see cref="IScope.SetVar"/> del scope actual de la sesion.
	/// </summary>
	/// <inheritdoc/>
	public override object VisitCreation([NotNull] CreationContext context)
	{
		VisitVaroperation(context.asignation(), Sesion.CurrentScope.SetVar);
		return true;
	}

	void VisitVaroperation(AsignationContext context, Action<string, dynamic, bool> actionVar)
	{
		var name = context.ID().GetText();
		var valueExp = context.expression();
		object val = _expressionVisitor.Visit(valueExp);

		actionVar(name, val, true);
	}

	/// <summary>
	/// Define una variable usando <see cref="IScope.DefVar"/> del scope actual de la sesion.
	/// </summary>
	/// <inheritdoc/>
	public override object VisitDefinition([NotNull] DefinitionContext context)
	{
		VisitVaroperation(context.asignation(), Sesion.CurrentScope.DefVar);

		return true;
	}

	/// <summary>
	/// Asigna el valor de <see cref="SimpleAssignContext.expression()"/> a la variable con nombre <see cref="SimpleAssignContext.ID()"/>
	/// </summary>
	/// <inheritdoc/>
	public override object VisitSimpleAssign([NotNull] SimpleAssignContext context)
	{
		VisitAsignation(context.ID(), context.expression(), context.Sequal());
		return true;
	}

	void VisitAsignation(IParseTree name, IParseTree expressionContext, IParseTree simbol)
	{ 
		var id = name.GetText();
		var operation = simbol.GetText();
		var value = _expressionVisitor.Visit(expressionContext);
		Sesion["LastToken"] = simbol;
		switch (operation)
		{
			case "=": break;
			case "+=":
				value = Sesion.CurrentScope.GetVar(id) + value;
				break;
			case "-=":
				value = Sesion.CurrentScope.GetVar(id) - value;
				break;
			case "*=":
				value = Sesion.CurrentScope.GetVar(id) * value;
				break;
			case "/=":
				value = Sesion.CurrentScope.GetVar(id) / value;
				break;
			case "%=":
				value = Sesion.CurrentScope.GetVar(id) % value;
				break;
			case "^=":
				value = Math.Pow(Sesion.CurrentScope.GetVar(id), value);
				break;
			case "~=":
				value = Math.Pow(value, 1d / Sesion.CurrentScope.GetVar(id));
				break;

		}
		Sesion.CurrentScope.AsigVar(id, value);
	}

	/// <summary>
	/// Calcula el valor de <see cref="PotencialAssignContext.expression()"/> elevado o como raiz con el valor que se encuentra en la variable <see cref="PotencialAssignContext.ID()"/> y lo asigna nuevamente a esa variable.
	/// </summary>
	/// <inheritdoc/>
	public override object VisitPotencialAssign([NotNull] PotencialAssignContext context)
	{
		VisitAsignation(context.ID(), context.expression(), context.POTENCIALASSIGN());
		return true;
	}

	/// <summary>
	/// Calcula el valor de <see cref="MultiplicativeAssignContext.expression()"/> multiplicado o dividido en el valor que se encuentra en la variable <see cref="MultiplicativeAssignContext.ID()"/> y lo asigna nuevamente a esa variable.
	/// </summary>
	/// <inheritdoc/>
	public override object VisitMultiplicativeAssign([NotNull] MultiplicativeAssignContext context)
	{
		VisitAsignation(context.ID(), context.expression(), context.MULTIPLICATIVEASSIGN());
		return true;
	}

	/// <summary>
	/// Calcula el valor de la variable <see cref="ModuleAssignContext.ID()"/> modulo el valor de <see cref="ModuleAssignContext.expression()"/> y lo asigna nuevamente a esa variable.
	/// </summary>
	/// <inheritdoc/>
	public override object VisitModuleAssign([NotNull] ModuleAssignContext context)
	{
		VisitAsignation(context.ID(), context.expression(), context.MODULEASSIGN());
		return true;
	}

	/// <summary>
	/// Calcula el valor de <see cref="AditiveAssignContext.expression()"/> sumado o restado en el valor que se encuentra en la variable <see cref="AditiveAssignContext.ID()"/> y lo asigna nuevamente a esa variable.
	/// </summary>
	/// <inheritdoc/>
	public override object VisitAditiveAssign([NotNull] AditiveAssignContext context)
	{
		VisitAsignation(context.ID(), context.expression(), context.ADITIVEASSIGN());
		return true;
	}
}
