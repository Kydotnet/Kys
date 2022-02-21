using System;
using System.Collections.Generic;
using System.Linq;
using KYLib.Modding;
using Kys.Lang;
using Kys.Library;
using Microsoft.Extensions.DependencyInjection;

namespace Kys;

[AutoLoad]
public class DefaultContextFactory : IContextFactory
{
	readonly IServiceProvider _serviceProvider;

	readonly Dictionary<ContextType, Func<IContext>> _factory = new();

	readonly IEnumerable<ContextType> _types =
		Enum.GetValues<ContextType>().Reverse();

	public DefaultContextFactory(IServiceProvider serviceProvider)
	{
		this._serviceProvider = serviceProvider;
		ChangeContext<RuntimeContext>(ContextType.All);
	}

	public void ChangeContext<T>(ContextType type) where T : IContext
	{
		_factory[type] = () =>
		{
			return ActivatorUtilities.CreateInstance<T>(_serviceProvider);
		};
	}

	public IContext Create(ContextType type)
	{
		if (_factory.ContainsKey(type))
			return _factory[type]();
		foreach (var item in _types)
		{
			if ((type & item) == item && _factory.ContainsKey(item))
				return _factory[item]();
		}
		return _factory[ContextType.All]();
	}
}
