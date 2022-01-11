using System.Reflection;

namespace Kys.Lang;

public sealed class KysFunction : IFunction
{

	/// <summary>
	/// Conjunto de sentencias que sera ejecutadas cuando se llame a la función.
	/// </summary>
	public IEnumerable<SentenceContext> Sentences { get; init; }

	public string Name { get; init; }

	public int ArgCount { get; init; }

	public bool InfArgs { get; init; }

	public IContext ParentContext { get; init; }

	/// <summary>
	/// Nombres de los parametros de la función.
	/// </summary>
	public string[] ParamsNames { get; init; }

	/// <summary>
	/// Este es el Visitor que se usa para ejecutar las sentencias dentro de la función
	/// </summary>
	public static KysParserBaseVisitor<bool> Executor { get; set; } = new();

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

		foreach (var item in Zip)
			FunctionScope.SetVar(item.First, item.Second, false);

		foreach (var sentence in Sentences)
			Executor.VisitSentence(sentence);

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
