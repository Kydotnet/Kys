using Kys.Interpreter;
using Kys.Library;
using Kys.Parser;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kys;
internal class KysProgram : BackgroundService
{
	KysParser ProgramParser;
	IInterpreter interpreter;
	IHostApplicationLifetime applicationLifetime;

	[ActivatorUtilitiesConstructor]
	public KysProgram(IHostApplicationLifetime applicationLifetime, KysParser programParser, IInterpreter interpreter, ILogger<KysProgram> logger)
	{
		ProgramParser = programParser;
		this.interpreter = interpreter;
		this.applicationLifetime = applicationLifetime;
	}

	public KysProgram(IHostApplicationLifetime applicationLifetime, KysParser programParser, IIterpreterBuilder interpreter, ILogger<KysProgram> logger) : 
		this(applicationLifetime, programParser, interpreter.Build(), logger)
	{

	}

	public KysProgram(IHostApplicationLifetime applicationLifetime, KysParser programParser, IServiceProvider contextFactory) : 
		this(applicationLifetime, programParser, KysInterpreter.CreateDefaultBuilder(contextFactory), null)
	{
	
	}

	protected override Task ExecuteAsync(CancellationToken stoppingToken)
	{
		return Task.Run(Start);
	}

	private void Start()
	{
		interpreter.Start(ProgramParser.program());
		applicationLifetime.StopApplication();
	}
}