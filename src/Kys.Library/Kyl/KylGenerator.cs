using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using KYLib.Utils;
using Kys.Lang;

namespace Kys.Library
{

	public static class KylGenerator
	{
		const BindingFlags stpublicmet = BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly;
		const BindingFlags publicmet = BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;


		internal static void Generate(string[] args)
		{
			var type = GetType(args);
			Ensure.NotNull(type, "Type");

			var smetv = from methods in type.GetMethods(stpublicmet)
						where !methods.IsSpecialName && !methods.IsGenericMethodDefinition && !methods.ContainsGenericParameters
						group methods by methods.Name into methodGroup
						select methodGroup.ToArray();

			var count = 0;
			foreach (var method in smetv)
			{
				if (method.Length == 1) // metodo unico
					IContext.Me.AddCsFunction(method[0]);
				else // metodo con sobrecargas
					IContext.Me.AddOverloadFunction(method);
				count++;
			}
		}

		public static void AddOverloadFunction(this IContext targetContext, MethodInfo[] methodOverloads)
		{
			var methods = methodOverloads.ToList();

			var name = methodOverloads[0].Name;
			

			var function = new OverloadFunction(methodOverloads)
			{
				Name = name,
				ParentContext = targetContext
			};

			targetContext.AddFunction(function);
			//function.Call(null,null, "34", 35);
		}

		private static Type GetType(string[] args)
		{
			if(args.Length == 1)
				return Type.GetType(args[0], false, true);
			var assembly = Assembly.Load(args[0]);
			return assembly?.GetType(args[1], false, true);
		}
	}
}
