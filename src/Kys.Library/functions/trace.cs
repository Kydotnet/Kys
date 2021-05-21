using System;
using System.Linq;

namespace Kys.Library
{
	partial class StandardFunctions
	{
		private delegate void d_trace(string obj, params object[] objs);

		private static readonly d_trace f_trace = Console.WriteLine;

		[Function("trace", -1)]
		private static dynamic trace(dynamic[] args)
		{
			if (args.Length == 0) Console.WriteLine();
			else if (args.Length == 1) Console.WriteLine($"{args[0]}");
			else
			{
				object[] par = args.Skip(1).ToArray();
				f_trace(args[0], par);
			}
			return null;
		}
	}
}