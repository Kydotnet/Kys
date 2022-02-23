#pragma warning disable CS1591, IDE1006
// ReSharper disable once CheckNamespace
namespace Kys.Library;

partial class StandardFunctions
{
	[Function(Name = "tarr")]
	public static Array Tarr(Type type, params dynamic[] args)
	{
		var ret = Array.CreateInstance(type, args.Length);
		var len = args.Length;
		for (var i = 0; i < len; i++)
			ret.SetValue(args[i], i);
		return ret;
	}
}
