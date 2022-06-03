using Kys.Runtime;

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
	public IKysParserVisitor<IKyObject> KysParserVisitor { get; }

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
		KysParserVisitor.VisitProgram(programContext);
	}

	/// <inheritdoc/>
	public void Stop()
	{
		throw new NotImplementedException();
	}
}
