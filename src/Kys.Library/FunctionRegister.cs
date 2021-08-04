using System;
using System.Linq;
using System.Reflection;
using KYLib.System;
using Kys.Lang;
using static Kys.Parser.KysParser;

namespace Kys.Library
{
	/// <summary>
	/// 
	/// </summary>
	public static partial class FunctionRegister
	{

		/// <summary>
		/// 
		/// </summary>
		/// <param name="targetContext"></param>
		public static void AddStandardFunctions(this IContext targetContext) =>
			AddFunctions(targetContext, typeof(StandardFunctions));

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="ContainerType"></typeparam>
		/// <param name="targetContext"></param>
		public static void AddFunctions<ContainerType>(this IContext targetContext) =>
			AddFunctions(targetContext, typeof(ContainerType));

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ContainerType"></param>
		/// <param name="targetContext"></param>
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

		/// <summary>
		/// 
		/// </summary>
		/// <param name="targetContext"></param>
		/// <param name="method"></param>
		public static void AddCsFunction(this IContext targetContext, MethodInfo method) =>
			AddCsFunction(targetContext, method, method.GetCustomAttribute<FunctionAttribute>() ?? FunctionAttribute.None);

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="targetContext"></param>
		/// <param name="method"></param>
		public static void AddCsFunction<T>(this IContext targetContext, T method)
			where T : Delegate =>
			AddCsFunction(targetContext, method.Method, method.Method.GetCustomAttribute<FunctionAttribute>() ?? FunctionAttribute.None);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="targetContext"></param>
		/// <param name="funcdefinition"></param>
		public static bool AddKysFunction(this IContext targetContext, FuncdefinitionContext funcdefinition)
		{
			var parameters = funcdefinition.parameters();
			var infargs = parameters.PARAMS()!=null;
			var @params = parameters.@params()?.ID().Select(id=>id.GetText()).ToArray();
			var haveparams = @params != null;
			var argcount = haveparams ?  @params.Length - (infargs?1:0) : 0;
			var ID = funcdefinition.ID().GetText();
			var sentences = funcdefinition.block().sentence();

			var function = new KysFunction()
			{
				Name = ID,
				ArgCount = argcount,
				InfArgs = infargs,
				ParentContext = targetContext,
				Sentences = sentences,
				ParamsNames = @params
			};

			return targetContext.AddFunction(function);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="targetContext"></param>
		/// <param name="method"></param>
		/// <param name="att"></param>
		private static void AddCsFunction(IContext targetContext, MethodInfo method, FunctionAttribute att)
		{
			var realmethod = IsAsync(method);

			var args = method.GetParameters();
			var argcount = args.Length;
			bool infArg = false;
			string name = att.Name ?? method.Name;

			if (argcount > 0 && args[^1].GetCustomAttribute<ParamArrayAttribute>() != null)
			{
				infArg = true;
				argcount--;
			}

			if (att.Passinfo)
			{
				if (argcount < 2)
					throw new ArgumentException($"El metodo {name} debe tener como minimo 2 argumentos ya que se establecio Passinfo en true.");
				if (!args[0].ParameterType.IsAssignableFrom(typeof(IContext)))
					throw new ArgumentException($"El primer parametro del metodo {name} debe poder aceptar objetos de tipo IContext.");
				if (!args[1].ParameterType.IsAssignableFrom(typeof(IScope)))
					throw new ArgumentException($"El segundo parametro del metodo {name} debe poder aceptar objetos de tipo IScope.");
				argcount -= 2;
			}

			var function = new CsFunction()
			{
				Method = realmethod,
				Name = name,
				ParentContext = targetContext,
				ArgCount = argcount,
				InfArgs = infArg
			};
			if (!targetContext.AddFunction(function))
				throw new ArgumentException($"A function with the name \"{function.Name}\" is already defined in the target context.");
		}
	}
}