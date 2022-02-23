using System;
using System.Threading;
using System.Threading.Tasks;
using Kys.Interpreter;
using Kys.Lang.Runtime;
using Kys.Parser;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
namespace Kys
{
	internal class KysHost : IHost
	{
		readonly IInterpreter _interpreter;

		public KysHost(IServiceProvider provider)
		{
			Services = provider;
			Swm.Step("Interpreter Creation");

			_interpreter = Services.GetService<IInterpreter>() ?? ActivatorUtilities.CreateInstance<KysInterpreter>(Services);
			
			Services.GetRequiredService<IInterpreterSesion>().Interpreter = _interpreter;

			Swm.Step("Interpreter Creation");
		}

		public IServiceProvider Services { get; }

		public void Dispose()
		{
		}

		public Task StartAsync(CancellationToken cancellationToken = default)
		{
			Swm.Step("Parser Creation");
			var programParser = Services.GetRequiredService<KysParser>();
			Swm.Step("Parser Creation");
			Swm.Step("Interpreter Configuration");
			_interpreter.ConfigureDefaultContext();
			Swm.Step("Interpreter Configuration");
			try
			{
				Swm.Step("Parsed");
				var parsed = programParser.program();
				Swm.Step("Parsed");
				Swm.Step("Kys Run");
				_interpreter.Start(parsed);
				Swm.Step("Kys Run");
			}
			catch (KysException e)
			{
				Console.WriteLine("{0} {1}", e.Line, e.Message);
			}
			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}
	}
}
