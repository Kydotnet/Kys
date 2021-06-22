using System;
using System.Linq;

namespace Kys.Library
{
	partial class StandardFunctions
	{
		[Function] static void trace(object obj, params dynamic[] objs)
		{
			if (objs.Length == 0) Console.WriteLine(obj);
			else Console.WriteLine(obj.ToString(), objs);
		}
	}
}