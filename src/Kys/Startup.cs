using System;
using System.IO;
using System.Linq;
using Antlr4.Runtime;
using Kys.Interpreter;
using Kys.Interpreter.Visitors;
using Kys.Lang;
using Kys.Lang.Runtime;
using Kys.Library;
using Kys.Parser;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using static Kys.Parser.KysParser;

namespace Kys;

internal class Startup
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
		SWM.Enabled = showtimes;
		SWM.Step("Program Run");

		Environment.SetEnvironmentVariable("KYS_PROGRAM_FILE", filename);

		SWM.Step("Host Configuration");
		var hostb = new KysHostBuilder()
			.ConfigureServices(ConfigureServices);
		SWM.Step("Host Configuration");
		SWM.Step("Host Creation");
		var host = hostb.Build();
		SWM.Step("Host Creation");
		SWM.Step("Host Run");
		host.Start();
		SWM.Step("Host Run");

		SWM.Step("Program Run");
		return Environment.ExitCode;

	}

	private static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
	{
		services.AddSingleton(S => S);

		services.AddSingleton(S => CharStreams.fromPath(Environment.GetEnvironmentVariable("KYS_PROGRAM_FILE")));
		services.AddSingleton<ITokenSource, KysLexer>();
		services.AddSingleton<ITokenStream, CommonTokenStream>();
		services.AddSingleton<IAntlrErrorListener<IToken>, KysErrorListener>();
		services.AddSingleton<KysParser>(S =>
		{
			var ret = new KysParser(S.GetRequiredService<ITokenStream>());
			ret.RemoveErrorListeners();
			ret.AddErrorListener(S.GetRequiredService<IAntlrErrorListener<IToken>>());
			return ret;
		});

		services.AddSingleton<IScopeFactory, DefaultScopeFactory>();
		services.AddSingleton<IContextFactory, DefaultContextFactory>();

		services.AddSingleton<IVisitorProvider>(ConfigureVisitors);
		services.AddSingleton<IInterpreterSesion, KysInterpreterSesion>();

		services.AddTransient<IContext>(S => S.GetRequiredService<IContextFactory>().Create(ContextType.ALL));
		services.AddTransient<IScope>(S => S.GetRequiredService<IScopeFactory>().Create(ScopeType.ALL));
	}

	private static KysVisitorProvider ConfigureVisitors(IServiceProvider arg)
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
