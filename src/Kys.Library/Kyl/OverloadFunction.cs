using System.Reflection;
#pragma warning disable CS8618
namespace Kys.Library.Kyl;

/// <summary>
/// Una función que recibe una longitud variada de parametros
/// </summary>
public sealed class OverloadFunction : IFunction
{
	readonly CsFunction[] _functions;

	/// <inheritdoc/>
	public string Name { get; init; }

	/// <inheritdoc/>
	public int ArgCount { get; }

	/// <inheritdoc/>
	public bool InfArgs { get; }

	/// <inheritdoc/>
	public IContext ParentContext { get; init; }

	internal OverloadFunction(IEnumerable<MethodInfo> methods, IContext targetContext)
	{
		var list = methods.
		Select(
			m => FunctionRegister.CreateCsFuntion(
				targetContext,
				m,
				m.GetCustomAttribute<FunctionAttribute>() ?? FunctionAttribute.None)
		).
		ToList();
		list.Sort(Sort);
		_functions = list.ToArray();
		InfArgs = list.Any(f => f.InfArgs);
		ArgCount = list.Min(f => f.ArgCount);
	}

	static int Sort(CsFunction f1, CsFunction f2)
	{
		if (f1.InfArgs != f2.InfArgs)
			return f1.InfArgs ? 1 : -1;
		if (f1.ArgCount != f2.ArgCount)
			return f1.ArgCount.CompareTo(f2.ArgCount);

		var pars1 = f1.Method.GetParameters();
		var pars2 = f2.Method.GetParameters();
		var f = 0;
		for (var i = 0; i < f1.ArgCount; i++)
		{
			var t1 = pars1[i].ParameterType;
			var t2 = pars2[i].ParameterType;
			if (t1.IsPrimitive && t2.IsPrimitive)
			{
				f += PrimitiveValueComparer(t1).CompareTo(PrimitiveValueComparer(t2));
				continue;
			}
			if ((t1.IsPrimitive && !t2.IsPrimitive) || t1.IsAssignableTo(t2))
				f--;
			if ((!t1.IsPrimitive && t2.IsPrimitive) || t2.IsAssignableTo(t1))
				f++;
		}
		return f;
	}

	static int PrimitiveValueComparer(Type t1)
	{
		if (t1 == typeof(char)) return 1;
		if (t1 == typeof(bool)) return 1;
		if (t1 == typeof(byte)) return 1;
		if (t1 == typeof(sbyte)) return 2;
		if (t1 == typeof(short)) return 3;
		if (t1 == typeof(ushort)) return 4;
		if (t1 == typeof(int)) return 5;
		if (t1 == typeof(uint)) return 6;
		if (t1 == typeof(long)) return 7;
		if (t1 == typeof(ulong)) return 8;
		if (t1 == typeof(float)) return 9;
		if (t1 == typeof(double)) return 10;
		return 0;
	}

	static int PrimitiveValueAssignation(Type t1)
	{
		if (t1 == typeof(char)) return 0;
		if (t1 == typeof(bool)) return 0;
		if (t1 == typeof(byte)) return -1;
		if (t1 == typeof(sbyte)) return 1;
		if (t1 == typeof(short)) return 2;
		if (t1 == typeof(ushort)) return -2;
		if (t1 == typeof(int)) return 3;
		if (t1 == typeof(uint)) return -3;
		if (t1 == typeof(long)) return 4;
		if (t1 == typeof(ulong)) return -4;
		if (t1 == typeof(float)) return 5;
		if (t1 == typeof(double)) return 6;
		return 0;
	}

	/// <inheritdoc/>
	public dynamic? Call(IContext callerContext, IScope functionScope, params dynamic?[] args) =>
		GetRealMethod(args).Call(callerContext, functionScope, args);

	CsFunction GetRealMethod(dynamic?[] args)
	{
		if (args.Length < ArgCount)
			return _functions[0];

		var filter = _functions.Where(
			f =>
				(f.ArgCount == args.Length && !f.InfArgs) ||
				(args.Length >= f.ArgCount && f.InfArgs)
		);
		var func = GetFunction(filter, args);
		if (func != null)
			return func;

		func = _functions.FirstOrDefault(f => f.ArgCount >= args.Length);
		return func ?? _functions[^1];

	}

	static CsFunction? GetFunction(IEnumerable<CsFunction> fixedFilter, IReadOnlyList<object?> args)
	{
		foreach (var func in fixedFilter)
		{
			var pars = func.Method.GetParameters();
			var fail = false;
			for (var i = 0; i < func.ArgCount; i++)
			{
				var hast1 = args[i] is not null;
				// tipo del objeto pasado
				var t1 = args[i]?.GetType() ?? typeof(object);
				// tipo esperado por la función
				var t2 = pars[i].ParameterType;
				// ambos son primitivos, si no se peude asignar falla.
				if (t1.IsPrimitive && t2.IsPrimitive)
				{
					if (IsAssignable(t1, t2))
						continue;
					fail = true;
					break;
				}
				if (hast1) // se paso un valor no nulo como parametro
				{
					if (t1.IsAssignableTo(t2))
						continue;
				}
				else if (!t2.IsValueType) // se esparaba un objeto de referencia por
				{
					// un valor referencia puede aceptar nulos por lo que podria ser aceptado.
					continue;
				}
				fail = true;
				break;
			}
			if (!fail) return func;
		}
		return null;
	}

	static bool IsAssignable(Type t1, Type t2)
	{
		var t1V = PrimitiveValueAssignation(t1);
		var t2V = PrimitiveValueAssignation(t2);
		if (t1V == 0 || t2V == 0) return false;
		if (t1V > 0) return t2V > t1V;
		return t2V < t1V || t2V > Math.Abs(t1V);
	}

	/// <inheritdoc/>
	public override string ToString() => $"{_functions[0].Method.ReturnType.Name} {Name}(...)";
}
