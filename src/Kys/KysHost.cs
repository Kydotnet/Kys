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

		readonly IInterpreterSesion _sesion;

		public KysHost(IServiceProvider provider)
		{
			Services = provider;
			Swm.Step("Interpreter Creation");

			_interpreter = Services.GetService<IInterpreter>() ?? ActivatorUtilities.CreateInstance<KysInterpreter>(Services);
			
			_sesion = Services.GetRequiredService<IInterpreterSesion>();
			_sesion.Interpreter = _interpreter;

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
				Console.WriteLine("line {0} [{1}] {2}", e.Line, e.Name ?? e.GetType().Name, e.Message);
			}
			catch (Exception ex)
			{
				var error = ex.InnerException ?? ex;
				Console.WriteLine("line {0}:{1} [{2}] {3}", _sesion["LastLine"] ?? 0, _sesion["LastColumn"] ?? 0, error.GetType().Name, error.Message);
#if  DEBUG
				Console.WriteLine(error.StackTrace);
#endif
			}
			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}
	}
}
