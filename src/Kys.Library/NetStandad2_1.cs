#if NETSTANDARD2_1_OR_GREATER
namespace Kys.Library;

internal static class NetStandard21
{
	internal static bool IsAssignableTo(this Type a, Type b)
	{
		return b.IsAssignableFrom(a);
	}
}

#endif