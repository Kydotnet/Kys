namespace Kys.Library
{
	public interface IScopeFactory
	{
		void ChangeScope<T>(ScopeFactoryType type) where T : IScope;

		IScope Create(ScopeFactoryType type, IScope parent = null);
	}
}