#pragma warning disable CS1591
namespace Kys.Library;

partial class StandardFunctions
{
	[Function]
	public static Array tarr(Type type, params dynamic[] args)
	{
		var ret = Array.CreateInstance(type, args.Length);
		args.CopyTo(ret, 0);
		return ret;
	}
}
