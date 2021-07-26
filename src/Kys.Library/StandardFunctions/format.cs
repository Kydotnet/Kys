#pragma warning disable CS1591
namespace Kys.Library
{
	partial class StandardFunctions
	{
		[Function] public static string format(string txt, params dynamic[] args) => string.Format(txt, args);
	}
}
