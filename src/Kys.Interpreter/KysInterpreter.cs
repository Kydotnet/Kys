using Kys.Parser;

namespace Kys.Interpreter;

public class KysInterpreter : IInterpreter
{
	public IContext ProgramContext { get; init; }

	public IInterpreterSesion Sesion { get; internal set; }

	public IKysParserVisitor<dynamic> KysParserVisitor { get; internal set; } 

	public static IIterpreterBuilder CreateDefaultBuilder(IServiceProvider services)
	{
		var dev =  new DefaultInterpreterBuilder(
			(IContextFactory)services.GetService(typeof(IContextFactory)), 
			(IInterpreterSesion)services.GetService(typeof(IInterpreterSesion)),
			(IVisitorProvider)services.GetService(typeof(IVisitorProvider))
		);
		return dev;
	}

	public void Start(ProgramContext programContext)
	{
		KysParserVisitor.VisitProgram(programContext);
	}

	public void Stop()
	{
		throw new NotImplementedException();
	}
}
