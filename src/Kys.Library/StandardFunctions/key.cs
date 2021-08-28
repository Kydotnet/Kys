#pragma warning disable CS1591
using System;

namespace Kys.Library
{
	partial class StandardFunctions
	{
		[Function]
		public static ConsoleKeyInfo key() => Console.ReadKey();
	}
}