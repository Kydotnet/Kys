using System;
using System.Collections.Generic;
using System.Linq;
using KYLib.Modding;
using Kys.Lang;
using Kys.Library;
using Microsoft.Extensions.DependencyInjection;
namespace Kys;

[AutoLoad]
public class DefaultScopeFactory : IScopeFactory
{
	readonly Dictionary<ScopeType, Func<IScope?, IScope>> _factory = new();

	readonly IEnumerable<ScopeType> _types =
		Enum.GetValues<ScopeType>().Reverse();
	readonly IServiceProvider _serviceProvider;

	public DefaultScopeFactory(IServiceProvider serviceProvider)
	{
		_serviceProvider = serviceProvider;
		ChangeScope<RuntimeScope>(ScopeType.All);
	}

	public void ChangeScope<T>(ScopeType type) where T : IScope =>
		_factory[type] = p =>
		{
			var scope = ActivatorUtilities.CreateInstance<T>(_serviceProvider);
			scope.ParentScope = p;
			return scope;
		};

	public IScope Create(ScopeType type, IScope? parent = null)
	{
		if (_factory.ContainsKey(type))
			return _factory[type](parent);
		foreach (var item in _types)
		{
			if ((type & item) == item && _factory.ContainsKey(item))
				return _factory[item](parent);
		}
		return _factory[ScopeType.All](parent);
	}
}
