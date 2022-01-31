using System.Reflection;
using KYLib.Utils;

namespace Kys.Library;

public static class KylGenerator
{
	const BindingFlags stpublicmet = BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly;
	const BindingFlags publicmet = BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;


	internal static void Generate(string[] args, IContext targetContext)
	{
		var type = GetType(args);
		Ensure.NotNull(type, "Type");

		var smetv = from methods in type.GetMethods(stpublicmet)
					where !methods.IsSpecialName && !methods.IsGenericMethodDefinition && !methods.ContainsGenericParameters
					group methods by methods.Name into methodGroup
					select methodGroup.ToArray();

		foreach (var method in smetv)
		{
			if (method.Length == 1) // metodo unico
				targetContext.AddCsFunction(method[0]);
			else // metodo con sobrecargas
				targetContext.AddOverloadFunction(method);
		}
	}

	public static void AddOverloadFunction(this IContext targetContext, MethodInfo[] methodOverloads)
	{
		var name = methodOverloads[0].Name;

		var sameName = methodOverloads.TrueForAll(m => m.Name == name);

		if (!sameName)
			throw new ArgumentException("All methods must be overloads of the same base method", nameof(methodOverloads));

		var function = new OverloadFunction(methodOverloads, targetContext)
		{
			Name = name,
			ParentContext = targetContext
		};

		targetContext.AddFunction(function);
	}

	private static Type GetType(string[] args)
	{
		if (args.Length == 1)
			return Type.GetType(args[0], false, true);
		var assembly = Assembly.Load(args[0]);
		return assembly?.GetType(args[1], false, true);
	}
}
