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
	private Dictionary<ScopeType, Func<IScope, IScope>> Factory = new();

	private IEnumerable<ScopeType> types =
		Enum.GetValues<ScopeType>().Reverse();
	private IServiceProvider serviceProvider;

	public DefaultScopeFactory(IServiceProvider serviceProvider)
	{
		this.serviceProvider = serviceProvider;
		ChangeScope<RuntimeScope>(ScopeType.ALL);
	}

	public void ChangeScope<T>(ScopeType type) where T : IScope =>
		Factory[type] = (p) => {
			var scope = ActivatorUtilities.CreateInstance<T>(serviceProvider);
			scope.ParentScope = p;
			return scope;
		};

	public IScope Create(ScopeType type, IScope parent = null)
	{
		if (Factory.ContainsKey(type))
			return Factory[type](parent);
		foreach (var item in types)
		{
			if ((type & item) == item && Factory.ContainsKey(item))
				return Factory[item](parent);
		}
		return Factory[ScopeType.ALL](parent);
	}
}
