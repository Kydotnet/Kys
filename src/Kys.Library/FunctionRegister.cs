using System;
using System.Collections;
using System.Reflection;
using Kys.Lang;

namespace Kys.Library
{
	/// <summary>
	/// 
	/// </summary>
	public static class FunctionRegister
	{
		/// <summary>
		/// 
		/// </summary>
		public static void AddStandardFunctions() => AddStandardFunctionsTo(IContext.Me);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="targetContext"></param>
		public static void AddStandardFunctionsTo(IContext targetContext) => AddFunctionsFromType(typeof(StandardFunctions), targetContext);

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="ContainerType"></typeparam>
		/// <param name="targetContext"></param>
		public static void AddFunctionsFromType<ContainerType>(IContext targetContext) =>
			AddFunctionsFromType(typeof(ContainerType), targetContext);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ContainerType"></param>
		/// <param name="targetContext"></param>
		public static void AddFunctionsFromType(Type ContainerType, IContext targetContext)
		{
			var methods = ContainerType.GetMethods(
				BindingFlags.DeclaredOnly
				| BindingFlags.NonPublic
				| BindingFlags.Public
				| BindingFlags.Static);
			foreach (var item in methods)
			{
				var att = item.GetCustomAttribute<FunctionAttribute>();
				if (att != null) PrivateAddFunction(targetContext, item, att);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="targetContext"></param>
		/// <param name="method"></param>
		/// <param name="att"></param>
		private static void PrivateAddFunction(IContext targetContext, MethodInfo method, FunctionAttribute att)
		{
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
				Method = method,
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