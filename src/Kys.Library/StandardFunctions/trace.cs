#pragma warning disable CS1591, IDE1006
// ReSharper disable once CheckNamespace
namespace Kys.Library;

partial class StandardFunctions
{
	[Function(Name = "trace")]
	public static void Trace(object? obj, params dynamic?[] objs)
	{
		if (objs.Length == 0) Console.WriteLine(obj);
		else if(obj is string format) Console.WriteLine(format, objs);
		else Console.WriteLine();
	}
}
