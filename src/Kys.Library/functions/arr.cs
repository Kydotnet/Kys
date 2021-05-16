using System;
using System.Linq;
namespace Kys.Library
{
	partial class StandardFunctions
	{
		[Function("arr", -1, true)]
		private static dynamic arr(dynamic[] args)
		{
			return args;
		}
	}
}