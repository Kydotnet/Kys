using Antlr4.Runtime;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Kys.Parser;
using Kys.Library;
using Kys.Lang;
using Kys.Interpreter;
using static Kys.Parser.KysParser;
using Kys.Interpreter.Visitors;
using System.Diagnostics;

namespace Kys;

internal class Startup
{
	public static int Main(string[] args)
	{
		var sw = Stopwatch.StartNew();
		if (args.Length > 0 && File.Exists(args[0]))
		{
			Environment.SetEnvironmentVariable("KYS_PROGRAM_FILE", args[0]);

			var sw1 = Stopwatch.StartNew();
			var host = Host.CreateDefaultBuilder(args)
				.ConfigureLogging(ConfigureLogging)
				.ConfigureAppConfiguration(ConfigureAppConfiguration)
				.ConfigureServices(ConfigureServices)
				.Build();
			Console.WriteLine("Host creation in {0}ms", sw1.ElapsedMilliseconds);
			sw1.Restart();
			host.Run();
			Console.WriteLine("Host run in {0}ms", sw1.ElapsedMilliseconds);
			sw1.Stop();


			Console.WriteLine("Program run in {0}ms", sw.ElapsedMilliseconds);
			sw.Stop();
			return Environment.ExitCode;
		}
		return -1;
	}

	private static void ConfigureAppConfiguration(HostBuilderContext context, IConfigurationBuilder configuration)
	{
		configuration.AddEnvironmentVariables("KYS_");		
	}

	private static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
	{
		services.AddSingleton(S => S);

		services.AddSingleton(S=>CharStreams.fromPath(S.GetRequiredService<IConfiguration>().GetRequiredSection("PROGRAM_FILE").Value));
		services.AddSingleton<ITokenSource, KysLexer>();
		services.AddSingleton<ITokenStream, CommonTokenStream>();
		services.AddSingleton<KysParser>();

		services.AddSingleton<IScopeFactory, DefaultScopeFactory>();
		services.AddSingleton<IContextFactory, DefaultContextFactory>();

		services.AddSingleton<IVisitorProvider>(ConfigureVisitors);
		services.AddSingleton<IInterpreterSesion, KysInterpreterSesion>();

		services.AddTransient<IContext>(S => S.GetRequiredService<IContextFactory>().Create(ContextFactoryType.ALL));
		services.AddTransient<IScope>(S => S.GetRequiredService<IScopeFactory>().Create(ScopeFactoryType.ALL));

		services.AddHostedService<KysProgram>();
	}

	private static KysVisitorProvider ConfigureVisitors(IServiceProvider arg)
	{
		var dev = new KysVisitorProvider(arg);

		dev.AddVisitor<ProgramContext, ProgramVisitor>();
		dev.AddVisitor<InstructionContext, InstructionVisitor>();
		dev.AddVisitor<ValueContext, ValueVisitor>();
		return dev;
	}

	private static void ConfigureLogging(HostBuilderContext arg1, ILoggingBuilder arg2)
	{
		// no se usan loggers por ahora
		arg2.ClearProviders();
#if DEBUG
		arg2.AddDebug();
#endif
	}
}
