namespace Kys.Interpreter;

/// <summary>
/// Implemetnación por defecto de <see cref="IInterpreter"/> que se ejecuta con el visitor dado.
/// </summary>
public class KysInterpreter : IInterpreter
{
	/// <inheritdoc/>
	public IContext ProgramContext { get; init; }

	/// <inheritdoc/>
	public IInterpreterSesion Sesion { get; set; }

	/// <summary>
	/// Visitor que sera usado para ejecutar el <see cref="KysParser.ProgramContext"/>.
	/// </summary>
	public IKysParserVisitor<dynamic> KysParserVisitor { get; set; }

	/// <inheritdoc/>
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
			Console.WriteLine("line {0}:{1} {2}", Sesion["LastLine"] ?? 0, Sesion["LastColumn"] ?? 0, ex.Message);
		}
	}

	/// <inheritdoc/>
	public void Stop()
	{
		throw new NotImplementedException();
	}
}
