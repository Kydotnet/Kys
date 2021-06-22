using System;

namespace Kys.Library
{
	partial class StandardFunctions
	{
		[Function] static Type type(string typeName) => Type.GetType(typeName, false, true);
	}
}