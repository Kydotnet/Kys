using Antlr4.Runtime.Misc;
using Kys.Runtime;

namespace Kys.Interpreter.Visitors;

/// <summary>
/// Implementación por defecto de <see cref="IVisitor{T}"/> para ejecutar <see cref="InstructionContext"/>.
/// </summary>
public class InstructionVisitor : BaseVisitor<IKyObject>
{
	#pragma warning disable CS8618
	IVisitor<IKyObject> _sentenceVisitor;
	#pragma warning restore CS8618
	
	/// <inheritdoc/>
	public override void Configure(IServiceProvider serviceProvider)
	{
		base.Configure(serviceProvider);
		_sentenceVisitor = VisitorProvider.GetVisitor<SentenceContext>();
	}

	/// <inheritdoc/>
	public override IKyObject VisitInstruction([NotNull] InstructionContext context)
	{
		Sesion["LastColumn"] = context.Start.Column;
		Sesion["LastLine"] = context.Start.Line;
		Sesion["LastInstruction"] = context;

		if (context.sentence() is SentenceContext sentence)
			return VisitSentence(sentence);
		if (context.funcdefinition() is FuncdefinitionContext funcdefinition)
			return VisitFuncdefinition(funcdefinition);
		if (context.exitprogram() is ExitprogramContext exitprogram)
			return VisitExitprogram(exitprogram);
		return Null;
	}

	/// <summary>
	/// Evalua <paramref name="context"/> con el <see cref="IVisitor{T}"/> para el contexto <see cref="SentenceContext"/>.
	/// </summary>
	/// <inheritdoc/>
	public override IKyObject VisitSentence([NotNull] SentenceContext context) =>
		_sentenceVisitor.VisitSentence(context);

	/// <summary>
	/// Genera una función Kys con <see cref="FunctionRegister.AddKysFunction(IContext, FuncdefinitionContext, IKysParserVisitor{object})"/>.
	/// </summary>
	/// <inheritdoc/>
	public override IKyObject VisitFuncdefinition([NotNull] FuncdefinitionContext context)
	{
		Sesion.CurrentContext.AddKysFunction(context, _sentenceVisitor);
		return True;
	}

	/// <summary>
	/// Establece <see cref="Environment.ExitCode"/> a <see cref="ExitprogramContext.NUMBER()"/> del <paramref name="context"/> y luego retorna finalizando el bucle principal de intruccciones.
	/// </summary>
	/// <inheritdoc/>
	public override IKyObject VisitExitprogram([NotNull] ExitprogramContext context)
	{
		var code = ValueVisitor.GetNumber(context.NUMBER());
		if(code is KyObject<int> Int)
			Environment.ExitCode = Int;
		return False;
	}
}
