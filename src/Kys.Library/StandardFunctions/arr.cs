#pragma warning disable CS1591, IDE1006
namespace Kys.Library;

partial class StandardFunctions
{
	[Function]
	public static Array arr(params dynamic[] args) => args;

	[Function]
	public static void setarr(Array array, object value, params int[] index) => array.SetValue(value, index);

	[Function]
	public static object getarr(Array array, params int[] indices) => array.GetValue(indices);

	[Function]
	public static int arrlen(Array array) => array.Length;
}
