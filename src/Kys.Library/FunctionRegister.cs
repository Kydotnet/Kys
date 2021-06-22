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
			ParamArrayAttribute a;
			if(att.Passinfo)
			{

			}
			else
			{
				var args = method.GetParameters();
				var argcount = args.Length;
				bool infArg = false;
				if(argcount > 0 && args[^1].GetCustomAttribute<ParamArrayAttribute>() != null)
				{
					infArg = true;
					argcount--;
				}
				var function = new CsFunction()
				{
					Method = method,
					Name = att.Name??method.Name,
					ParentContext = targetContext,
					ArgCount = argcount,
					InfArgs = infArg
				};
				if(!targetContext.AddFunction(function))
					throw new ArgumentException($"A function with the name \"{function.Name}\" is already defined in the target context.");
			}
		}
	}
}