using Kys.Lang;
using Kys.Library;

namespace Kys;

public class DefaultScopeFactory : IScopeFactory
{
	public void ChangeScope<T>(ScopeType type) where T : IScope, new() { }

	public IScope Create(ScopeType type, IScope? parent = null)
	{
		return new RuntimeScope
		{
			ParentScope = parent
		};
	}
}
