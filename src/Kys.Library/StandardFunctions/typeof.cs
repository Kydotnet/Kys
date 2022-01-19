#pragma warning disable CS1591
namespace Kys.Library;

partial class StandardFunctions
{
	[Function(Name = "typeof")]
	public static Type Typeof(object obj) => obj?.GetType();
}
