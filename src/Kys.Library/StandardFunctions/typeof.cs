using System;

namespace Kys.Library
{
	partial class StandardFunctions
	{
		[Function(Name ="typeof")]
		static Type Typeof(dynamic obj) => obj?.GetType();
	}
}