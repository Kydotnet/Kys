#pragma warning disable CS1591, IDE1006
// ReSharper disable once CheckNamespace
namespace Kys.Library;

partial class StandardFunctions
{
	[Function(Name = "format")]
	public static string Format(string txt, params dynamic[] args) => string.Format(txt, args);
}
