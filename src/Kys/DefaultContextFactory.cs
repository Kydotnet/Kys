using KYLib.Modding;
using Kys.Lang;
using Kys.Library;

namespace Kys;

[AutoLoad]
public class DefaultContextFactory : IContextFactory
{
	readonly IScopeFactory _serviceProvider;

	public DefaultContextFactory(IScopeFactory serviceProvider)
	{
		_serviceProvider = serviceProvider;
	}

	public void ChangeContext<T>(ContextType type) where T : IContext
	{
	}

	public IContext Create(ContextType type)
	{
		return type switch
		{
			ContextType.All => new RuntimeContext(_serviceProvider.Create(ScopeType.All)),
			ContextType.Me => new RuntimeContext(_serviceProvider.Create(ScopeType.Me)),
			ContextType.Kys => new RuntimeContext(_serviceProvider.Create(ScopeType.Kys)),
			ContextType.Package => new RuntimeContext(_serviceProvider.Create(ScopeType.Package)),
			_ => throw new NotImplementedException(),
		};
	}
}

