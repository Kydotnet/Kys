using Antlr4.Runtime.Misc;

namespace Kys.Interpreter.Visitors;

/// <summary>
/// Implementación por defecto de <see cref="IVisitor{T}"/> para ejecutar <see cref="InstructionContext"/>.
/// </summary>
public class InstructionVisitor : BaseVisitor<object>
{
	IVisitor<object> sentenceVisitor;

	/// <inheritdoc/>
	public override void Configure(IServiceProvider serviceProvider)
	{
		base.Configure(serviceProvider);
		sentenceVisitor = VisitorProvider.GetVisitor<SentenceContext>();
	}

	/// <inheritdoc/>
	public override object VisitInstruction([NotNull] InstructionContext context)
	{
		Sesion["LastColumn"] = context.Start.Column;
		Sesion["LastLine"] = context.Start.Line;
		Sesion["LastInstruction"] = context;
		return base.VisitInstruction(context);
	}

	/// <summary>
	/// Evalua <paramref name="context"/> con el <see cref="IVisitor{T}"/> para el contexto <see cref="SentenceContext"/>.
	/// </summary>
	/// <inheritdoc/>
	public override object VisitSentence([NotNull] SentenceContext context) =>
		sentenceVisitor.VisitSentence(context);

	/// <summary>
	/// Genera una función Kys con <see cref="FunctionRegister.AddKysFunction(IContext, FuncdefinitionContext, IKysParserVisitor{object})"/>.
	/// </summary>
	/// <inheritdoc/>
	public override object VisitFuncdefinition([NotNull] FuncdefinitionContext context)
	{
		Sesion.CurrentContext.AddKysFunction(context, sentenceVisitor);
		return null;
	}

	/// <summary>
	/// Establece <see cref="Environment.ExitCode"/> a <see cref="ExitprogramContext.NUMBER()"/> del <paramref name="context"/> y luego retorna finalizando el bucle principal de intruccciones.
	/// </summary>
	/// <inheritdoc/>
	public override object VisitExitprogram([NotNull] ExitprogramContext context)
	{
		double code = ValueVisitor.GetNumber(context.NUMBER());
		int exit = (int)Math.Round(code);
		Environment.ExitCode = exit;
		return false;
	}
}
