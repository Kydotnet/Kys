#if NETSTANDARD2_1_OR_GREATER
namespace Kys.Library;

internal static class netstandad2_1
{
	internal static bool IsAssignableTo(this Type a, Type b)
	{
		return b.IsAssignableFrom(a);
	}
}

#endif