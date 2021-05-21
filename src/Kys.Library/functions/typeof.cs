namespace Kys.Library
{
	partial class StandardFunctions
	{
		[Function("typeof", 1)]
		private static dynamic Typeof(dynamic[] args) =>
			args[0]?.GetType();
	}
}