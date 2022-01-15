namespace Kys.Interpreter;

public class KysInterpreter : IInterpreter
{
	public IContext ProgramContext { get; init; }

	public IInterpreterSesion Sesion { get; set; }

	public IKysParserVisitor<dynamic> KysParserVisitor { get; set; }

	public void ConfigureContext()
	{
		this.ConfigureDefaultContext();
	}

	public void Start(ProgramContext programContext)
	{
		Sesion.CallerContext = null;
		Sesion.CurrentScope = ProgramContext.RootScope;
		Sesion.CurrentContext = ProgramContext;

		try
		{
			KysParserVisitor.VisitProgram(programContext);
		}
		catch (Exception ex)
		{
			Console.WriteLine("line {0}:{1} {2}", Sesion["LastLine"],Sesion["LastColumn"], ex.Message);
		}
	}

	public void Stop()
	{
		throw new NotImplementedException();
	}
}
