using System.Reflection;

namespace Kys.Lang;

/// <summary>
/// Una función creada directamente por el interprete con sentecias Kys.
/// </summary>
public sealed class KysFunction : IFunction
{

	/// <summary>
	/// Conjunto de sentencias que sera ejecutadas cuando se llame a la función.
	/// </summary>
	public IEnumerable<SentenceContext> Sentences { get; init; }

	/// <inheritdoc/>
	public string Name { get; init; }

	/// <inheritdoc/>
	public int ArgCount { get; init; }

	/// <inheritdoc/>
	public bool InfArgs { get; init; }

	/// <inheritdoc/>
	public IContext ParentContext { get; init; }

	/// <summary>
	/// Nombres de los parametros de la función.
	/// </summary>
	public string[] ParamsNames { get; init; }

	/// <summary>
	/// Este es el Visitor que se usa para ejecutar las sentencias dentro de la función
	/// </summary>
	public IKysParserVisitor<object> SenteceVisitor { get; init; }

	/// <inheritdoc/>
	public dynamic Call(IContext CallerContext, IScope FunctionScope, params dynamic[] args)
	{
		ValidateParams(args.Length);
		
		//FunctionScope.Start();
		var temp = ParamsNames;

		if (InfArgs)
		{
			temp = ParamsNames.SkipLast(1).ToArray();
			var param = args.Skip(ArgCount).ToArray();
			FunctionScope.SetVar(ParamsNames[^1], param);
		}

#if NET5_0_OR_GREATER
			var Zip = temp.Zip(args);
#elif NETSTANDARD2_1_OR_GREATER
		var Zip = temp.Zip(args, (First, Second) => (First, Second));
#endif

		foreach (var (First, Second) in Zip)
			FunctionScope.SetVar(First, Second, false);

		foreach (var sentence in Sentences)
			SenteceVisitor.VisitSentence(sentence);

		FunctionScope.DefVar("return", null, false);
		var ret = FunctionScope.GetVar("return", false);

		//FunctionScope.Stop();

		return ret;
	}

	/// <summary>
	/// Validamos que la cantidad de parametros sea la esperada.
	/// </summary>
	private void ValidateParams(int length)
	{
		if (ArgCount == -1)
			return;
		if (InfArgs)
		{
			if (length < ArgCount)
				throw new TargetParameterCountException();
		}
		else
			if (length != ArgCount)
			throw new TargetParameterCountException();
	}
}
