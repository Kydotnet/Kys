using System;
using System.Collections;
using System.Reflection;
using Kys.Lang;

namespace Kys.Library
{
	public static class FunctionRegister
	{
		public static void Standard(IContext functions)
		{
			AddFunctions(typeof(StandardFunctions), functions);
		}

		public static void AddFunctions(Type ContainerType, IContext functions)
		{
			var methods = ContainerType.GetMethods(
				BindingFlags.DeclaredOnly
				| BindingFlags.NonPublic
				| BindingFlags.Public
				| BindingFlags.Static);
			foreach (var item in methods)
			{
				var att = item.GetCustomAttribute<FunctionAttribute>();
				if (att != null) AddFunction(functions, item, att);
			}
		}

		private static void AddFunction(IContext functions, MethodInfo item, FunctionAttribute att)
		{
			ParamArrayAttribute a;
			/*Func func = item.CreateDelegate<Func>();

			functions.Add(att.Name, new Function()
			{
				Name = att.Name,
				ArgCount = att.Argcount,
				Method = func
			});*/
		}
	}
}