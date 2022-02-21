namespace Kys.Lang
{
	internal static class ScopeExtensions
	{
		public static IScope? CheckRecursive(this IScope start, string id)
		{
			while (true)
			{
				if (start.ConVar(id))
					return start;
				if (start.ParentScope == null)
					return null;
				start = start.ParentScope;
			}
		}
	}
}
