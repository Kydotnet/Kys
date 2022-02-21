using System;
using System.IO;
using System.Linq;
using Antlr4.Runtime;
using Kys.Interpreter;
using Kys.Interpreter.Visitors;
using Kys.Lang.Runtime;
using Kys.Library;
using Kys.Parser;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using static Kys.Parser.KysParser;

namespace Kys;

internal static class Startup
{
	public static int Main(string[] args)
	{
		if (args.Length > 0 && File.Exists(args[0]))
		{
			var showtime = args.Contains("-t");
			var filename = args[0];

			return Entry(filename, showtime);
		}
		return -1;
	}

	public static int Entry(string filename, bool showtimes)
	{
		Swm.Enabled = showtimes;
		Swm.Step("Program Run");

		Environment.SetEnvironmentVariable("KYS_PROGRAM_FILE", filename);

		Swm.Step("Host Configuration");
		var hostb = new KysHostBuilder()
			.ConfigureServices(ConfigureServices);
		Swm.Step("Host Configuration");
		Swm.Step("Host Creation");
		var host = hostb.Build();
		Swm.Step("Host Creation");
		Swm.Step("Host Run");
		host.Start();
		Swm.Step("Host Run");

		Swm.Step("Program Run");
		return Environment.ExitCode;

	}

	static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
	{
		services.AddSingleton(s => s);

		services.AddSingleton(_ => CharStreams.fromPath(Environment.GetEnvironmentVariable("KYS_PROGRAM_FILE")));
		services.AddSingleton<ITokenSource, KysLexer>();
		services.AddSingleton<ITokenStream, CommonTokenStream>();
		services.AddSingleton<IAntlrErrorListener<IToken>, KysErrorListener>();
		services.AddSingleton(s =>
		{
			var ret = new KysParser(s.GetRequiredService<ITokenStream>());
			ret.RemoveErrorListeners();
			ret.AddErrorListener(s.GetRequiredService<IAntlrErrorListener<IToken>>());
			return ret;
		});

		services.AddSingleton<IScopeFactory, DefaultScopeFactory>();
		services.AddSingleton<IContextFactory, DefaultContextFactory>();

		services.AddSingleton<IVisitorProvider>(ConfigureVisitors);
		services.AddSingleton<IInterpreterSesion, KysInterpreterSesion>();

		services.AddSingleton(s => s.GetRequiredService<IContextFactory>().Create(ContextType.Me));
		
		services.AddSingleton(s => s.GetRequiredService<IScopeFactory>().Create(ScopeType.Me));
	}

	static KysVisitorProvider ConfigureVisitors(IServiceProvider arg)
	{
		var dev = new KysVisitorProvider(arg);

		dev.AddVisitor<ProgramContext, ProgramVisitor>();

		dev.AddVisitor<InstructionContext, InstructionVisitor>();
		dev.AddVisitor<ExitprogramContext, InstructionVisitor>();
		dev.AddVisitor<FuncdefinitionContext, InstructionVisitor>();

		dev.AddVisitor<ValueContext, ValueVisitor>();
		dev.AddVisitor<FuncresultContext, ValueVisitor>();

		dev.AddVisitor<ExpressionContext, ExpressionVisitor>();

		dev.AddVisitor<SentenceContext, SentenceVisitor>();
		dev.AddVisitor<FunccallContext, SentenceVisitor>();

		dev.AddVisitor<VaroperationContext, VaroperationVisitor>();
		dev.AddVisitor<DeclarationContext, VaroperationVisitor>();
		dev.AddVisitor<CreationContext, VaroperationVisitor>();
		dev.AddVisitor<DefinitionContext, VaroperationVisitor>();
		dev.AddVisitor<SelfasignationContext, VaroperationVisitor>();

		dev.AddVisitor<ControlContext, ControlVisitor>();
		dev.AddVisitor<BlockContext, ControlVisitor>();

		return dev;
	}
}
