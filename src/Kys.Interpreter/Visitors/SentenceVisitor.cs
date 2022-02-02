using Antlr4.Runtime.Misc;

namespace Kys.Interpreter.Visitors;

/// <summary>
/// Implementación por defecto de <see cref="IVisitor{T}"/> para ejecutar <see cref="SentenceContext"/>.
/// </summary>
public class SentenceVisitor : BaseVisitor<object>
{
	IKysParserVisitor<dynamic> controlVisitor;
	IKysParserVisitor<dynamic> varOperationVisitor;
	IKysParserVisitor<dynamic> funcresultVisitor;

	/// <inheritdoc/>
	public override void Configure(IServiceProvider serviceProvider)
	{
		base.Configure(serviceProvider);
		controlVisitor = VisitorProvider.GetVisitor<ControlContext>();
		varOperationVisitor = VisitorProvider.GetVisitor<VaroperationContext>();
		funcresultVisitor = VisitorProvider.GetVisitor<FuncresultContext>();
	}

	/// <inheritdoc/>
	public override object VisitSentence([NotNull] SentenceContext context)
	{
		Sesion["LastColumn"] = context.Start.Column;
		Sesion["LastLine"] = context.Start.Line;
		Sesion["LastSentence"] = context;
		return base.VisitSentence(context);
	}

	/// <summary>
	/// Se evalua <paramref name="context"/> usando el <see cref="IVisitor{T}"/> para el contexto <see cref="ControlContext"/>.
	/// </summary>
	/// <inheritdoc/>
	public override object VisitControl([NotNull] ControlContext context) =>
		controlVisitor.VisitControl(context);

	/// <summary>
	/// Se evalua <paramref name="context"/> usando el <see cref="IVisitor{T}"/> para el contexto <see cref="VaroperationContext"/>.
	/// </summary>
	/// <inheritdoc/>
	public override object VisitVaroperation([NotNull] VaroperationContext context) =>
		varOperationVisitor.VisitVaroperation(context);

	/// <summary>
	/// Se evalua <see cref="FunccallContext.funcresult()"/> de <paramref name="context"/> usando el <see cref="IVisitor{T}"/> para el contexto <see cref="FuncresultContext"/>.
	/// </summary>
	/// <inheritdoc/>
	public override object VisitFunccall([NotNull] FunccallContext context)
	{
		funcresultVisitor.VisitFuncresult(context.funcresult());

		// si se devuelve algo que no sea nulo el InstructionVisitor terminara
		return null;
	}
}
