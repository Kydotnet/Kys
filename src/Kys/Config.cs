using System;
using System.Linq;
using Kys.Core;

namespace Kys
{
	public static class Config
	{
		public static void ConfigFuncs()
		{
			Program.Functions.Add("trace", new()
			{
				Name = "trace",
				Method = Trace,
				ArgCount = -1,
				HasReturn = false
			});
			Program.Functions.Add("clear", new()
			{
				Name = "clear",
				Method = Clear,
				ArgCount = 0,
				HasReturn = false,
			});
		}

		private static dynamic Clear(dynamic[] args)
		{
			Console.Clear();
			return null;
		}

		#region trace

		private delegate void d_trace(string obj, params object[] objs);

		private static dynamic Trace(dynamic[] args)
		{
			if (args.Length == 0) Console.WriteLine();
			else if (args.Length == 1) Console.WriteLine(args[0] as object);
			else
			{
				object[] par = args.Skip(1).ToArray();
				d_trace trace = Console.WriteLine;
				trace(args[0], par);
			}
			return null;
		}

		#endregion
	}
}