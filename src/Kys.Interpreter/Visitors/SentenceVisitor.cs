using Antlr4.Runtime.Misc;
namespace Kys.Interpreter.Visitors;

/// <summary>
/// Implementación por defecto de <see cref="IVisitor{T}"/> para ejecutar <see cref="SentenceContext"/>.
/// </summary>
public class SentenceVisitor : BaseVisitor<object>
{
	#pragma warning disable CS8618
	IKysParserVisitor<dynamic> _controlVisitor;
	IKysParserVisitor<dynamic> _varOperationVisitor;
	IKysParserVisitor<dynamic> _funcresultVisitor;
	#pragma warning restore CS8618
	
	/// <inheritdoc/>
	public override void Configure(IServiceProvider serviceProvider)
	{
		base.Configure(serviceProvider);
		_controlVisitor = VisitorProvider.GetVisitor<ControlContext>();
		_varOperationVisitor = VisitorProvider.GetVisitor<VaroperationContext>();
		_funcresultVisitor = VisitorProvider.GetVisitor<FuncresultContext>();
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
		_controlVisitor.VisitControl(context);

	/// <summary>
	/// Se evalua <paramref name="context"/> usando el <see cref="IVisitor{T}"/> para el contexto <see cref="VaroperationContext"/>.
	/// </summary>
	/// <inheritdoc/>
	public override object VisitVaroperation([NotNull] VaroperationContext context) =>
		_varOperationVisitor.VisitVaroperation(context);

	/// <summary>
	/// Se evalua <see cref="FunccallContext.funcresult()"/> de <paramref name="context"/> usando el <see cref="IVisitor{T}"/> para el contexto <see cref="FuncresultContext"/>.
	/// </summary>
	/// <inheritdoc/>
	public override object VisitFunccall([NotNull] FunccallContext context)
	{
		_funcresultVisitor.VisitFuncresult(context.funcresult());

		// si se false el InstructionVisitor terminara
		return true;
	}
}
