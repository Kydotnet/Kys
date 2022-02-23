#pragma warning disable CS1591, IDE1006
// ReSharper disable once CheckNamespace
namespace Kys.Library;

partial class StandardFunctions
{
	[Function(Name = "type")] 
	public static Type? Type(string typeName) => System.Type.GetType(typeName, false, true);
}
