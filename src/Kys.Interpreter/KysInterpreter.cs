namespace Kys.Interpreter;

/// <summary>
/// Implementación por defecto de <see cref="IInterpreter"/> que se ejecuta con el visitor dado.
/// </summary>
public class KysInterpreter : IInterpreter
{
	/// <inheritdoc/>
	public IContext ProgramContext { get; }

	/// <inheritdoc/>
	public IInterpreterSesion Sesion { get; }

	/// <summary>
	/// Visitor que sera usado para ejecutar el <see cref="KysParser.ProgramContext"/>.
	/// </summary>
	public IKysParserVisitor<dynamic> KysParserVisitor { get; }

	/// <summary>
	/// Crea una nueva instancia de interprete de Kys.
	/// </summary>
	/// <param name="programContext">Contexto principal del programa.</param>
	/// <param name="sesion">Sesión para usar en el interprete.</param>
	/// <param name="visitorProvider">Visitor que se usara para ejecutar el programa.</param>
	public KysInterpreter(IContext programContext, IInterpreterSesion sesion, IVisitorProvider visitorProvider)
	{
		ProgramContext = programContext;
		Sesion = sesion;
		KysParserVisitor = visitorProvider.GetVisitor<ProgramContext>();
		Sesion.CallerContext = null;
		Sesion.CurrentScope = ProgramContext.RootScope;
		Sesion.CurrentContext = ProgramContext;
	}

	/// <inheritdoc/>
	public void Start(ProgramContext programContext)
	{
		try
		{
			KysParserVisitor.VisitProgram(programContext);
		}
		catch (Exception ex)
		{
			var error = ex.InnerException ?? ex;
			Console.WriteLine("line {0}:{1} [{2}] {3}", Sesion["LastLine"] ?? 0, Sesion["LastColumn"] ?? 0, error.GetType().Name, error.Message);
#if  DEBUG
			Console.WriteLine(error.StackTrace);
#endif
		}
	}

	/// <inheritdoc/>
	public void Stop()
	{
		throw new NotImplementedException();
	}
}
