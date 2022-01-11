namespace Kys.Lang
{
	internal static class IScopeExtensions
	{
		public static IScope CheckRecursive(this IScope Start, string ID)
		{
			if (Start.ConVar(ID))
				return Start;
			if (Start.ParentScope != null)
				return CheckRecursive(Start.ParentScope, ID);
			return null;
		}
	}
}
