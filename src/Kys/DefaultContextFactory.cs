using KYLib.Modding;
using Kys.Lang;
using Kys.Library;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kys;

[AutoLoad]
public class DefaultContextFactory : IContextFactory
{
	IServiceProvider serviceProvider;

	private Dictionary<ContextType, Func<IContext>> Factory = new();

	private IEnumerable<ContextType> types =
		Enum.GetValues<ContextType>().Reverse();

	public DefaultContextFactory(IServiceProvider serviceProvider)
	{
		this.serviceProvider = serviceProvider;
		ChangeContext<RuntimeContext>(ContextType.ALL);
	}

	public void ChangeContext<T>(ContextType type) where T : IContext
	{
		Factory[type] = () => {
			return ActivatorUtilities.CreateInstance<T>(serviceProvider);
		};
	}

	public IContext Create(ContextType type)
	{
		if (Factory.ContainsKey(type))
			return Factory[type]();
		foreach (var item in types)
		{
			if ((type & item) == item && Factory.ContainsKey(item))
				return Factory[item]();
		}
		return Factory[ContextType.ALL]();
	}
}
