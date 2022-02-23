#pragma warning disable CS1591
// ReSharper disable once CheckNamespace
namespace Kys.Library;

partial class StandardFunctions
{
	[Function(Name = "typeof")]
	public static Type? Typeof(object? obj) => obj?.GetType();
}
