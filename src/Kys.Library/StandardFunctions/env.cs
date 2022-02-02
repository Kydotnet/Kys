#pragma warning disable CS1591
using System.Collections;

namespace Kys.Library;

partial class StandardFunctions
{
	[Function]
	public static void setenv(string name, object value) => Environment.SetEnvironmentVariable(name, value?.ToString());

	[Function]
	public static string getenv(string name) => Environment.GetEnvironmentVariable(name);

	[Function]
	public static IDictionary lsenv() => Environment.GetEnvironmentVariables();
}
