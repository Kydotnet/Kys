using System;

namespace Kys.Library
{
	partial class StandardFunctions
	{
		[Function]
		static Array arr(params dynamic[] args) => args;
	}
}