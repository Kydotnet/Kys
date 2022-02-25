#pragma warning disable CS1591, IDE1006
// ReSharper disable once CheckNamespace
namespace Kys.Library;

partial class StandardFunctions
{
	[Function(Name = "equals")]
	public static bool KEquals(dynamic? a, dynamic? b)
	{
		if (a is null && b is null) return true;
		return a?.Equals(b) ?? b!.Equals(a);
	}

	[Function(Name = "same")]
	public static bool Same(object? a, object? b)
	{
		if (a is null && b is null) return true;
		return ReferenceEquals(a, b);
	}
}