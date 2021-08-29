#pragma warning disable CS1591
namespace Kys.Library;

partial class StandardFunctions
{
	[Function]
	public static ConsoleKeyInfo key() => Console.ReadKey();
}
