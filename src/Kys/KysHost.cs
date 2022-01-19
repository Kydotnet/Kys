using Kys.Interpreter;
using Kys.Lang.Runtime;
using Kys.Library;
using Kys.Parser;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using static Kys.Parser.KysParser;

namespace Kys
{
	internal class KysHost : IHost
	{
		IInterpreter interpreter;

		public KysHost(IServiceProvider provider)
		{
			Services = provider;
		}

		public IServiceProvider Services { get; private set; }

		public void Dispose()
		{
		}

		public Task StartAsync(CancellationToken cancellationToken = default)
		{
			SWM.Step("Interpreter Creation");
			var ProgramParser = Services.GetRequiredService<KysParser>();
			interpreter = new KysInterpreter()
			{
				KysParserVisitor = Services.GetService<IVisitorProvider>().GetVisitor<ProgramContext>(),
				ProgramContext = Services.GetService<IContextFactory>().Create(ContextType.ME),
				Sesion = Services.GetService<IInterpreterSesion>()
			};			
			SWM.Step("Interpreter Creation");
			SWM.Step("Interpreter Configuration");
			interpreter.ConfigureDefaultContext();
			SWM.Step("Interpreter Configuration");
			try
			{
				SWM.Step("Parsed");
				var parsed = ProgramParser.program();
				SWM.Step("Parsed");
				SWM.Step("Kys Run");
				interpreter.Start(parsed);
				SWM.Step("Kys Run");
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
