using System;
using System.Linq;
using Kys.Core;

namespace Kys
{
	public static class Config
	{
		public static void ConfigFuncs()
		{
			Func trace = Trace;
			Program.Functions.Add("trace", new()
			{
				Method = trace,
				ArgCount = -1,
				HasReturn = false
			});
		}

		private delegate void d_trace(string obj, params object[] objs);

		private static dynamic Trace(dynamic[] args)
		{
			if (args.Length == 0) Console.WriteLine();
			if (args.Length == 1) Console.WriteLine(args[0] as object);
			else
			{
				object[] par = args.Skip(1).ToArray();
				d_trace trace = Console.WriteLine;
				trace(args[0], par);
			}
			return null;
		}
	}
}