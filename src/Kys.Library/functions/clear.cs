using System;

namespace Kys.Library
{
	partial class StandardFunctions
	{

		[Function("clear", 0, false)]
		private static dynamic Clear(dynamic[] args)
		{
			Console.Clear();
			return null;
		}
	}
}