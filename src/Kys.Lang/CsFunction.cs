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
			if (PassInfo)
			{
				FunctionScope.Start();
				var temp = new dynamic[realargs.Length + 2];
				temp[0] = CallerContext;
				temp[1] = FunctionScope;
				realargs.CopyTo(temp, 2);
				var ret = Method.Invoke(null, temp);
				FunctionScope.Stop();
				return ret;
			}
			else
			{
				return Method.Invoke(null, realargs);
			}
		}

		private dynamic[] GetRealArgs(dynamic[] args)
		{
			if (!InfArgs) return args;
			if (args.Length < ArgCount) return args;
			var ret = new dynamic[ArgCount + 1];
			args[0..ArgCount].CopyTo(ret, 0);
			ret[^1] = args.Skip(ArgCount).ToArray();
			return ret;
		}
	}
}