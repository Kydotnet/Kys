﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Atn;
using Kys.Lang;

namespace Kys.Library
{
	partial class FunctionRegister
	{
		internal static CsFunction CreateCsFuntion(IContext context, MethodInfo method, FunctionAttribute att)
		{
			CheckSupport(method);
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

			return new CsFunction()
			{
				Method = realmethod,
				Name = name,
				ParentContext = context,
				ArgCount = argcount,
				InfArgs = infArg
			};
		}

		private static void CheckSupport(MethodInfo methodInfo)
		{
			if(methodInfo.IsSpecialName)
				throw new NotSupportedException("Special name methods are not supported in kys");
			if(methodInfo.IsGenericMethodDefinition || methodInfo.ContainsGenericParameters)
				throw new NotSupportedException("Generic methods not defined are not supported in kys");
		}
	}
}