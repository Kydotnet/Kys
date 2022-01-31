using System.Reflection;

namespace Kys.Library;

public sealed class OverloadFunction : IFunction
{
	private CsFunction[] Functions;

	public string Name { get; init; }

	public int ArgCount { get; private set; }

	public bool InfArgs { get; private set; }

	public IContext ParentContext { get; init; }

	internal OverloadFunction(MethodInfo[] methods, IContext targetContext)
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
		Functions = list.ToArray();
		InfArgs = list.Any(f => f.InfArgs);
		ArgCount = list.Min(f => f.ArgCount);
	}

	private static int Sort(CsFunction f1, CsFunction f2)
	{
		if (f1.InfArgs != f2.InfArgs)
			return f1.InfArgs ? 1 : -1;
		if (f1.ArgCount != f2.ArgCount)
			return f1.ArgCount.CompareTo(f2.ArgCount);

		var pars1 = f1.Method.GetParameters();
		var pars2 = f2.Method.GetParameters();
		var f = 0;
		for (int i = 0; i < f1.ArgCount; i++)
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

	private static int PrimitiveValueComparer(Type t1)
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

	private static int PrimitiveValueAssignation(Type t1)
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

	public dynamic Call(IContext CallerContext, IScope FunctionScope, params dynamic[] args) =>
		GetRealMethod(args).Call(CallerContext, FunctionScope, args);

	private CsFunction GetRealMethod(dynamic[] args)
	{
		if (args.Length < ArgCount)
			return Functions[0];

		var Filter = Functions.Where(
			f =>
				(f.ArgCount == args.Length && !f.InfArgs) ||
				(args.Length >= f.ArgCount && f.InfArgs)
		);
		var Func = GetFunction(Filter, args);
		if (Func != null)
			return Func;

		Func = Functions.FirstOrDefault(f => f.ArgCount >= args.Length);
		if (Func != null)
			return Func;

		return Functions[^1];
	}

	private CsFunction GetFunction(IEnumerable<CsFunction> fixedFilter, object[] args)
	{
		foreach (var func in fixedFilter)
		{
			var pars = func.Method.GetParameters();
			var fail = false;
			for (int i = 0; i < func.ArgCount; i++)
			{
				var t1 = args[i].GetType();
				var t2 = pars[i].ParameterType;
				if (t1.IsPrimitive && t2.IsPrimitive)
				{
					if (isAssignable(t1, t2))
						continue;
					else
					{
						fail = true;
						break;
					}
				}
				if (!t1.IsAssignableTo(t2))
				{
					fail = true;
					break;
				}
			}
			if (!fail) return func;
		}
		return null;
	}

	private bool isAssignable(Type t1, Type t2)
	{
		var t1v = PrimitiveValueAssignation(t1);
		var t2v = PrimitiveValueAssignation(t2);
		if (t1v == 0 || t2v == 0) return false;
		if (t1v > 0) return t2v > t1v;
		if (t1v < 0) return t2v < t1v || t2v > Math.Abs(t1v);

		return false;
	}

	public override string ToString() => $"{Functions[0].Method.ReturnType.Name} {Name}(...)";
}
