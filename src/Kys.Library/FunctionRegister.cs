using Kys.Parser;
using System.Reflection;

namespace Kys.Library;

/// <inheritdoc/>
public static partial class FunctionRegister
{
	/// <summary>
	/// Agrega a <paramref name="targetContext"/> todas los metodos disponibles en el tipo <see cref="StandardFunctions"/>.
	/// </summary>
	/// <param name="targetContext">Contetxo de destino en donde se agregaran las funciones.</param>
	public static void AddStandardFunctions(this IContext targetContext) =>
		AddFunctions(targetContext, typeof(StandardFunctions));

	/// <summary>
	/// Busca en el tipo <typeparamref name="ContainerType"/> todos los metodos estaticos que tengan <see cref="FunctionAttribute"/> y los agrega a <paramref name="targetContext"/> como <see cref="IFunction"/>.
	/// </summary>
	/// <param name="targetContext">Contetxo de destino en donde se agregaran las funciones.</param>
	/// <typeparam name="ContainerType">El tipo del cual se buscan y extraen los metodos.</typeparam>
	public static void AddFunctions<TContainerType>(this IContext targetContext) =>
		AddFunctions(targetContext, typeof(TContainerType));

	/// <summary>
	/// Busca en el tipo <paramref name="containerType"/> todos los metodos estaticos que tengan <see cref="FunctionAttribute"/> y los agrega a <paramref name="targetContext"/> como <see cref="IFunction"/>.
	/// </summary>
	/// <param name="targetContext">Contetxo de destino en donde se agregaran las funciones.</param>
	/// <param name="containerType">El tipo del cual se buscan y extraen los metodos.</param>
	public static void AddFunctions(this IContext targetContext, Type containerType)
	{
		var methods = containerType.GetMethods(
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

	/// <summary>
	/// Agrega una funci�n definida en Kys al contexto ded destino.
	/// </summary>
	/// <param name="targetContext">Contetxo en el que sera almacenada la funci�n.</param>
	/// <param name="funcdefinition">Definici�n de la funci�n.</param>
	/// <param name="sentenceVisitor">Visitor que se usara para ejecutar las sentencias internas de la funci�n.</param>
	public static void AddKysFunction(this IContext targetContext, FuncdefinitionContext funcdefinition, IKysParserVisitor<object> sentenceVisitor)
	{
		var parameters = funcdefinition.parameters();
		var infargs = parameters.PARAMS() != null;
		var @params = parameters.@params()?.ID().Select(id => id.GetText()).ToArray();
		var haveparams = @params != null;
		var argcount = haveparams ? @params.Length - (infargs ? 1 : 0) : 0;
		var id = funcdefinition.ID().GetText();
		var sentences = funcdefinition.block().sentence();

		if (!haveparams && infargs)
		{
			@params = new string[] { "params" };
		}

		var function = new KysFunction
		{
			Name = id,
			ArgCount = argcount,
			InfArgs = infargs,
			ParentContext = targetContext,
			Sentences = sentences,
			ParamsNames = @params,
			SenteceVisitor = sentenceVisitor,
		};

		if (!targetContext.AddFunction(function))
			throw new ArgumentException($"A function with the name \"{function.Name}\" is already defined in the target context.");
	}

	/// <summary>
	/// Convierte el metodo <paramref name="method"/> en una <see cref="IFunction"/> invocable en Kys y la agrega a <paramref name="targetContext"/>.
	/// </summary>
	/// <param name="targetContext"><see cref="IContext"/> de destino al cual se agregara la funci�n generada.</param>
	/// <param name="method">Metodo de C# que se convertira, este debe ser un metodo estatico de lo contrario se producira un error al intentar ejecutar la funci�n ya que se estara tratando de invocar sin instancia.</param>
	public static void AddCsFunction(this IContext targetContext, MethodInfo method) =>
		AddCsFunction(targetContext, method, method.GetCustomAttribute<FunctionAttribute>() ?? FunctionAttribute.None);

	/// <summary>
	/// Convierte el metodo <paramref name="method"/> en una <see cref="IFunction"/> invocable en Kys y la agrega a <paramref name="targetContext"/>.
	/// </summary>
	/// <typeparam name="T">Tipo de delegado que contiene el metodo que se usara.</typeparam>
	/// <param name="targetContext"><see cref="IContext"/> de destino al cual se agregara la funci�n generada.</param>
	/// <param name="method">Metodo de C# que se convertira, este debe ser un metodo estatico de lo contrario se producira un error al intentar ejecutar la funci�n ya que se estara tratando de invocar sin instancia.</param>
	public static void AddCsFunction<T>(this IContext targetContext, T method)
		where T : Delegate =>
		AddCsFunction(targetContext, method.Method, method.Method.GetCustomAttribute<FunctionAttribute>() ?? FunctionAttribute.None);

	static void AddCsFunction(IContext targetContext, MethodInfo method, FunctionAttribute att)
	{
		var function = CreateCsFuntion(targetContext, method, att);

		if (!targetContext.AddFunction(function))
			throw new ArgumentException($"A function with the name \"{function.Name}\" is already defined in the target context.");
	}
}
