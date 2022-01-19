namespace Kys.Library
{
	public interface IScopeFactory
	{
		void ChangeScope<T>(ScopeType type) where T : IScope;

		IScope Create(ScopeType type, IScope parent = null);
	}
}