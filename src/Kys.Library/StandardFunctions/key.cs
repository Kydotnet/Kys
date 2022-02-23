#pragma warning disable CS1591, IDE1006
// ReSharper disable once CheckNamespace
namespace Kys.Library;

partial class StandardFunctions
{
	[Function(Name = "key")]
	public static ConsoleKeyInfo Key() => Console.ReadKey();
}
