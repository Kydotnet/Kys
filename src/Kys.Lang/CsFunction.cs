using Kys.Runtime;
using System.Diagnostics;
using System.Reflection;
#pragma warning disable CS8618

namespace Kys.Lang;

/// <summary>
/// Contenedor simple para llamar un metodo estatico de C# desde Kys.
/// </summary>
[DebuggerDisplay("{Method}")]
public sealed class CsFunction : IFunction
{
	/// <summary>
	/// Representa un metodo de C#
	/// </summary>
	public MethodInfo Method { get; init; }

	/// <inheritdoc/>
	public string Name { get; init; }

	/// <inheritdoc/>
	public int ArgCount { get; init; }

	/// <inheritdoc/>
	public bool InfArgs { get; init; }

	/// <summary>
	/// Indica si a la función se le debe pasar información del llamado.
	/// </summary>
	public bool PassInfo { get; init; }

	/// <inheritdoc/>
	public IContext ParentContext { get; init; }

	/// <inheritdoc/>
	public IKyObject Call(IContext callerContext, IScope functionScope, params IKyObject[] args)
	{
		var realargs = GetRealArgs(args);
		if (!PassInfo) return FromUnknowValue(Method.Invoke(null, realargs));
		var temp = new dynamic[realargs.Length + 2];
		temp[0] = callerContext;
		temp[1] = functionScope;
		realargs.CopyTo(temp, 2);
		var ret = Method.Invoke(null, temp);
		return FromUnknowValue(ret);
	}

	/// <summary>
	/// Comprime los argumentos finales en uno solo en caso de tratarse de argumentos infinitos.
	/// </summary>
	/// <param name="entry"></param>
	/// <returns></returns>
	object?[] GetRealArgs(IKyObject[] entry)
	{
		var args = entry.Select(E => E.ToCsharp()).ToArray();
		if (!InfArgs || args.Length < ArgCount) return args;
		var ret = new object[ArgCount + 1];
		args[..ArgCount].CopyTo(ret, 0);
		var par = args.Skip(ArgCount).ToArray();
		ret[^1] = Array.CreateInstance(
			Method.GetParameters()[^1].ParameterType.GetElementType()!,
			par.Length
		);
		Array.Copy(par, (Array)ret[^1], par.Length);
		return ret;
	}
}
