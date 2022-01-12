using Kys.Parser;
using System.Reflection;

namespace Kys.Library;

public static partial class FunctionRegister
{

	public static void AddStandardFunctions(this IContext targetContext) =>
		AddFunctions(targetContext, typeof(StandardFunctions));

	public static void AddFunctions<ContainerType>(this IContext targetContext) =>
		AddFunctions(targetContext, typeof(ContainerType));

	public static void AddFunctions(this IContext targetContext, Type ContainerType)
	{
		var methods = ContainerType.GetMethods(
			BindingFlags.DeclaredOnly
			| BindingFlags.NonPublic
			| BindingFlags.Public
			| BindingFlags.Static);
		foreach (var item in methods)
		{
			var att = item.GetCustomAttribute<FunctionAttribute>();
			if (att != null)
				AddCsFunction(targetContext, item, att);
		}
	}

	public static void AddKysFunction(this IContext targetContext, FuncdefinitionContext funcdefinition, IKysParserVisitor<object> sentenceVisitor)
	{
		var parameters = funcdefinition.parameters();
		var infargs = parameters.PARAMS() != null;
		var @params = parameters.@params()?.ID().Select(id => id.GetText()).ToArray();
		var haveparams = @params != null;
		var argcount = haveparams ? @params.Length - (infargs ? 1 : 0) : 0;
		var ID = funcdefinition.ID().GetText();
		var sentences = funcdefinition.block().sentence();

		if(!haveparams && infargs)
		{
			@params = new string[]{ "params" };
		}

		var function = new KysFunction()
		{
			Name = ID,
			ArgCount = argcount,
			InfArgs = infargs,
			ParentContext = targetContext,
			Sentences = sentences,
			ParamsNames = @params,
			senteceVisitor = sentenceVisitor,
		};

		if (!targetContext.AddFunction(function))
			throw new ArgumentException($"A function with the name \"{function.Name}\" is already defined in the target context.");
	}

	public static void AddCsFunction(this IContext targetContext, MethodInfo method) =>
	AddCsFunction(targetContext, method, method.GetCustomAttribute<FunctionAttribute>() ?? FunctionAttribute.None);

	public static void AddCsFunction<T>(this IContext targetContext, T method)
		where T : Delegate =>
		AddCsFunction(targetContext, method.Method, method.Method.GetCustomAttribute<FunctionAttribute>() ?? FunctionAttribute.None);

	private static void AddCsFunction(IContext targetContext, MethodInfo method, FunctionAttribute att)
	{
		var function = CreateCsFuntion(targetContext, method, att);

		if (!targetContext.AddFunction(function))
			throw new ArgumentException($"A function with the name \"{function.Name}\" is already defined in the target context.");
	}
}
