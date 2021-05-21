using System;
using System.Linq;

namespace Kys.Library
{
	partial class StandardFunctions
	{
		private delegate string d_format(string obj, params object[] objs);

		private static readonly d_format f_format = string.Format;

		[Function("format", -1)]
		private static dynamic format(dynamic[] args)
		{
			if (args.Length == 0) return string.Empty;
			return f_format(args[0], args.Skip(1).ToArray());
		}
	}
}
