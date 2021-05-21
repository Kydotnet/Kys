using System;

namespace Kys.Library
{
	partial class StandardFunctions
	{
		[Function("type", 1)]
		private static dynamic type(dynamic[] args) =>
			Type.GetType(args[0], false, true);
	}
}