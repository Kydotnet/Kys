using KYLib.Modding;
using Kys.Lang;
using Kys.Library;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kys;

[AutoLoad]
public class DefaultScopeFactory : IScopeFactory
{
	private Dictionary<ScopeFactoryType, Func<IScope, IScope>> Factory = new();

	private IEnumerable<ScopeFactoryType> types =
		Enum.GetValues<ScopeFactoryType>().Reverse();
	private IServiceProvider serviceProvider;

	public DefaultScopeFactory(IServiceProvider serviceProvider)
	{
		this.serviceProvider = serviceProvider;
		ChangeScope<RuntimeScope>(ScopeFactoryType.ALL);
	}

	public void ChangeScope<T>(ScopeFactoryType type) where T : IScope =>
		Factory[type] = (p) => {
			var scope = ActivatorUtilities.CreateInstance<T>(serviceProvider);
			scope.ParentScope = p;
			return scope;
		};

	public IScope Create(ScopeFactoryType type, IScope parent = null)
	{
		if (Factory.ContainsKey(type))
			return Factory[type](parent);
		foreach (var item in types)
		{
			if ((type & item) == item && Factory.ContainsKey(item))
				return Factory[item](parent);
		}
		return Factory[ScopeFactoryType.ALL](parent);
	}
}
