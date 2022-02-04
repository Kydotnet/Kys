#pragma warning disable CS1591, IDE1006
namespace Kys.Library;

partial class StandardFunctions
{
	[Function] public static Type type(string typeName) => Type.GetType(typeName, false, true);
}
