using System;
using System.Collections.Generic;
using KYLib.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
namespace Kys;

internal class KysHostBuilder : IHostBuilder
{

	public IDictionary<object, object> Properties { get; } = new Dictionary<object, object>();
	readonly List<Action<HostBuilderContext, IServiceCollection>> _configureServicesActions = new();

	IServiceCollection? _services;
	IServiceProvider? _servicesProvider;

	public IHost Build()
	{
		Swm.Step("Host Service Creation");
		BuildServiceProvider();
		Swm.Step("Host Service Creation");
		var host = new KysHost(_servicesProvider!);
		return host;
	}

	void BuildServiceProvider()
	{
		_services = new ServiceCollection();
		var host = new HostBuilderContext(Properties);
		
		foreach (var item in _configureServicesActions)
		{
			item(host, _services);
		}
		_servicesProvider = _services.BuildServiceProvider();
	}

	public IHostBuilder ConfigureAppConfiguration(Action<HostBuilderContext, IConfigurationBuilder> configureDelegate)
	{
		return this;
	}

	public IHostBuilder ConfigureContainer<TContainerBuilder>(Action<HostBuilderContext, TContainerBuilder> configureDelegate)
	{
		return this;
	}

	public IHostBuilder ConfigureHostConfiguration(Action<IConfigurationBuilder> configureDelegate)
	{
		return this;
	}

	public IHostBuilder ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureDelegate)
	{
		Ensure.NotNull(configureDelegate, nameof(configureDelegate));
		_configureServicesActions.Add(configureDelegate);
		return this;
	}

	public IHostBuilder UseServiceProviderFactory<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> factory) where TContainerBuilder : notnull
	{
		return this;
	}

	public IHostBuilder UseServiceProviderFactory<TContainerBuilder>(Func<HostBuilderContext, IServiceProviderFactory<TContainerBuilder>> factory) where TContainerBuilder : notnull
	{
		return this;
	}
}
