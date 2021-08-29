using System.Reflection;

namespace Kys.Library;

public sealed class OverloadFunction : IFunction
{
	private CsFunction[] Functions;

	public string Name { get; init; }

	public int ArgCount { get; init; }

	public int MaxArgCount { get; init; }

	public bool InfArgs { get; init; }

	public IContext ParentContext { get; init; }

	public OverloadFunction(MethodInfo[] methods)
	{
		var list = methods.
		Select(
			m => FunctionRegister.CreateCsFuntion(
				IContext.Me,
				m,
				m.GetCustomAttribute<FunctionAttribute>() ?? FunctionAttribute.None)
		).
		ToList();
		list.Sort(Sort);

		Functions = list.ToArray();
	}

	private static int Sort(CsFunction f1, CsFunction f2)
	{
		if (f1.InfArgs != f2.InfArgs)
			return f1.InfArgs ? 1 : -1;
		if (f1.ArgCount != f2.ArgCount)
			return f1.ArgCount.CompareTo(f2.ArgCount);
		else
		{
			var pars1 = f1.Method.GetParameters();
			var pars2 = f2.Method.GetParameters();
			var f = 0;
			for (int i = 0; i < f1.ArgCount; i++)
			{
				var t1 = pars1[i].ParameterType;
				var t2 = pars2[i].ParameterType;
				if (t1.IsAssignableTo(t2))
					f--;
				if (t2.IsAssignableTo(t1))
					f++;
			}
			return f;
		}
	}

	public dynamic Call(IContext CallerContext, IScope FunctionScope, params dynamic[] args) =>
		GetRealMethod(args).Call(CallerContext, FunctionScope, args);

	private CsFunction GetRealMethod(dynamic[] args)
	{
		var Filter = Functions.Where(
			f =>
				(f.ArgCount == args.Length && !f.InfArgs) ||
				(args.Length >= f.ArgCount && f.InfArgs)
		);
		var Func = GetFunction(Filter, args);

		if (Func != null)
			return Func;

		return null;
	}

	private CsFunction GetFunction(IEnumerable<CsFunction> fixedFilter, object[] args)
	{
		foreach (var func in fixedFilter)
		{
			var pars = func.Method.GetParameters();
			var fail = false;
			for (int i = 0; i < func.ArgCount; i++)
			{
				if (!args[i].GetType().IsAssignableTo(pars[i].ParameterType))
				{
					fail = true;
					break;
				}
			}
			if (!fail) return func;
		}

		return null;
	}

	public override string ToString() => $"{Functions[0].Method.ReturnType.Name} {Name}(...)";
}
