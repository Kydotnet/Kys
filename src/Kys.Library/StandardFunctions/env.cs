#pragma warning disable CS1591, IDE1006
using System.Collections;

// ReSharper disable once CheckNamespace
namespace Kys.Library;

partial class StandardFunctions
{
	[Function(Name = "setenv")]
	public static void Setenv(string name, object? value) => Environment.SetEnvironmentVariable(name, value?.ToString());

	[Function(Name = "getenv")]
	public static string? Getenv(string name) => Environment.GetEnvironmentVariable(name);

	[Function(Name = "lsenv")]
	public static IDictionary Lsenv() => Environment.GetEnvironmentVariables();
}
