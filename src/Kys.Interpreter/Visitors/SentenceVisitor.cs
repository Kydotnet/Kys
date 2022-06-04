using Antlr4.Runtime.Misc;
using Kys.Runtime;

namespace Kys.Interpreter.Visitors;

/// <summary>
/// Implementación por defecto de <see cref="IVisitor{T}"/> para ejecutar <see cref="SentenceContext"/>.
/// </summary>
public class SentenceVisitor : BaseVisitor<IKyObject>
{
	#pragma warning disable CS8618
	IKysParserVisitor<IKyObject> _controlVisitor;
	IKysParserVisitor<IKyObject> _varOperationVisitor;
	IKysParserVisitor<IKyObject> _funcresultVisitor;
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
	public override IKyObject VisitSentence([NotNull] SentenceContext context)
	{
		Sesion["LastColumn"] = context.Start.Column;
		Sesion["LastLine"] = context.Start.Line;
		Sesion["LastSentence"] = context;

		if (context.Control is ControlContext sentence)
			return VisitControl(sentence);
		if (context.Varoperation is VaroperationContext funcdefinition)
			return VisitVaroperation(funcdefinition);
		if (context.Funccall is FunccallContext exitprogram)
			return VisitFunccall(exitprogram);
		return Null;
	}
	
	/// <summary>
	/// Se evalua <paramref name="context"/> usando el <see cref="IVisitor{T}"/> para el contexto <see cref="ControlContext"/>.
	/// </summary>
	/// <inheritdoc/>
	public override IKyObject VisitControl([NotNull] ControlContext context) =>
		_controlVisitor.VisitControl(context);

	/// <summary>
	/// Se evalua <paramref name="context"/> usando el <see cref="IVisitor{T}"/> para el contexto <see cref="VaroperationContext"/>.
	/// </summary>
	/// <inheritdoc/>
	public override IKyObject VisitVaroperation([NotNull] VaroperationContext context) =>
		_varOperationVisitor.VisitVaroperation(context);

	/// <summary>
	/// Se evalua <see cref="FunccallContext.funcresult()"/> de <paramref name="context"/> usando el <see cref="IVisitor{T}"/> para el contexto <see cref="FuncresultContext"/>.
	/// </summary>
	/// <inheritdoc/>
	public override IKyObject VisitFunccall([NotNull] FunccallContext context)
	{
		_funcresultVisitor.VisitFuncresult(context.Funcresult);

		// si se devuelve false el InstructionVisitor terminara
		return True;
	}
}
