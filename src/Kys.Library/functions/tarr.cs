using System;
using System.Linq;

namespace Kys.Library
{
	partial class StandardFunctions
	{
		[Function("tarr", -1)]
		private static dynamic tarr(dynamic[] args)
		{
			if (args.Length < 1)
				throw new ArgumentException($"La función tarr requiere 1 parametro pero se pasaron {args.Length}");

			Type type = args[0];

			var ret = Array.CreateInstance(type, args.Length - 1);

			args.Skip(1).ToArray().CopyTo(ret, 0);

			return ret;
		}
	}
}