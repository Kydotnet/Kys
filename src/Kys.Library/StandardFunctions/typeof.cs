#pragma warning disable CS1591
using System;

namespace Kys.Library
{
	partial class StandardFunctions
	{
		[Function(Name ="typeof")]
		public static Type Typeof(dynamic obj) => obj?.GetType();
	}
}