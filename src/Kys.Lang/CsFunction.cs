using System;
using System.Reflection;

namespace Kys.Lang
{
	public class CsFunction : IFunction
	{
		/// <summary>
		/// Representa un metodo de C#
		/// </summary>
		public MethodInfo Method { get; init; }

		public string Name { get; init; }

		public int ArgCount { get; init; }

		public bool InfArgs { get; init; }

		/// <summary>
		/// Indica si a la función se le debe pasar información del llamado.
		/// </summary>
		public bool PassInfo { get; init; }

		public IContext ParentContext { get; init; }

		public dynamic Call(IContext CallerContext, IScope FunctionScope, params dynamic[] args)
		{
			if (PassInfo)
			{
				FunctionScope.Start();
				var temp = new dynamic[args.Length + 2];
				temp[0] = CallerContext;
				temp[1] = FunctionScope;
				args.CopyTo(temp, 2);
				var ret = Method.Invoke(null, temp);
				FunctionScope.Stop();
				return ret;
			}
			else
			{
				return Method.Invoke(null, args);
			}
		}
	}
}