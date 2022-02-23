using System.Reflection;
#pragma warning disable CS8618

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
	public string[]? ParamsNames { get; init; }

	/// <summary>
	/// Este es el Visitor que se usa para ejecutar las sentencias dentro de la función
	/// </summary>
	public IKysParserVisitor<object> SenteceVisitor { get; init; }

	/// <inheritdoc/>
	public dynamic? Call(IContext callerContext, IScope functionScope, params dynamic?[] args)
	{
		ValidateParams(args.Length);

		if (ParamsNames is not null)
		{
			var temp = ParamsNames;

			if (InfArgs)
			{
				temp = ParamsNames.SkipLast(1).ToArray();
				var param = args.Skip(ArgCount).ToArray();
				functionScope.SetVar(ParamsNames[^1], param);
			}
			if (ArgCount > 0 && !InfArgs)
			{
				var zip = temp.Zip(args, (first, second) => (First: first, Second: second));
				foreach (var (first, second) in zip)
					functionScope.SetVar(first, second, false);
			}
		}

		foreach (var sentence in Sentences)
			SenteceVisitor.VisitSentence(sentence);

		functionScope.DefVar("return", null, false);
		var ret = functionScope.GetVar("return", false);

		return ret;
	}

	/// <summary>
	/// Validamos que la cantidad de parametros sea la esperada.
	/// </summary>
	void ValidateParams(int length)
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
