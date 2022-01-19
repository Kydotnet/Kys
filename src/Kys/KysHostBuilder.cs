using KYLib.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;

namespace Kys;

internal class KysHostBuilder : IHostBuilder
{

	public IDictionary<object, object> Properties { get; } = new Dictionary<object, object>();
	readonly List<Action<HostBuilderContext, IServiceCollection>> ConfigureServicesActions = new();

	IServiceCollection services;
	IServiceProvider servicesProvider;

	public KysHostBuilder()
	{

	}

	public IHost Build()
	{
		SWM.Step("Host Service Creation");
		BuildServiceProvider();
		SWM.Step("Host Service Creation");
		var host = new KysHost(servicesProvider);
		return host;
	}

	private void BuildServiceProvider()
	{
		services = new ServiceCollection();
		foreach (var item in ConfigureServicesActions)
		{
			item(null, services);
		}
		servicesProvider = services.BuildServiceProvider();
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
		ConfigureServicesActions.Add(configureDelegate);
		return this;
	}

	public IHostBuilder UseServiceProviderFactory<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> factory)
	{
		return this;
	}

	public IHostBuilder UseServiceProviderFactory<TContainerBuilder>(Func<HostBuilderContext, IServiceProviderFactory<TContainerBuilder>> factory)
	{
		return this;
	}
}
