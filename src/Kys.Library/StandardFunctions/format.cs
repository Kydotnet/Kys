namespace Kys.Library
{
	partial class StandardFunctions
	{
		[Function] static string format(string txt, params dynamic[] args) => string.Format(txt, args);
	}
}
