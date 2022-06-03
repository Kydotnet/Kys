#pragma warning disable CS1591, IDE1006
// ReSharper disable once CheckNamespace
namespace Kys.Library;

partial class StandardFunctions
{
	[Function(Name = "arr")]
	public static Array Arr(params dynamic[] args) => args;

	[Function(Name = "setarr")]
	public static void Setarr(Array array, object value, params int[] index) => array.SetValue(value, index);

	[Function(Name = "getarr")]
	public static object? Getarr(Array array, params int[] indices) => array.GetValue(indices);

	[Function(Name = "arrlen")]
	public static int Arrlen(Array array) => array.Length;
}
