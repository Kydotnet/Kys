#pragma warning disable CS1591
using System;
using System.Linq;

namespace Kys.Library
{
	partial class StandardFunctions
	{
		[Function]
		public static void trace(object obj, params dynamic[] objs)
		{
			if (objs.Length == 0) Console.WriteLine(obj);
			else Console.WriteLine(obj.ToString(), objs);
		}
	}
}