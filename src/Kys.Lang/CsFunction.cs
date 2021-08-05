using System;
using System.Linq;
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
			var realargs = GetRealArgs(args);
			if (! PassInfo) return Method.Invoke(null, realargs);
			FunctionScope.Start();
			var temp = new dynamic[realargs.Length + 2];
			temp[0] = CallerContext;
			temp[1] = FunctionScope;
			realargs.CopyTo(temp, 2);
			var ret = Method.Invoke(null, temp);
			FunctionScope.Stop();
			return ret;
		}

		private dynamic[] GetRealArgs(dynamic[] args)
		{
			if (!InfArgs || args.Length < ArgCount) return args;
			var ret = new object[ArgCount + 1];
			args[0..ArgCount].CopyTo(ret, 0);
			var par = args.Skip(ArgCount).ToArray();
			ret[^1] = Array.CreateInstance(
				Method.GetParameters()[^1].ParameterType.GetElementType(),
				par.Length
			);
			Array.Copy(par, (Array)ret[^1], par.Length);
			return ret;
		}
	}
}