#pragma warning disable CS1591
using System;

namespace Kys.Library
{
	partial class StandardFunctions
	{
		[Function]
		public static Array arr(params dynamic[] args) => args;
	}
}