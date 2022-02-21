namespace Kys.Interpreter;

/// <summary>
/// Un objeto que se encarga de iniciar la ejución d eun programa Kys apartir de un <see cref="ProgramContext"/>, el interprete tambien es el encargado de el prevenir la propagación de errores provocados por Kys.
/// </summary>
public interface IInterpreter
{
	/// <summary>
	/// El contexto principal del programa, este valor no deberia de cambiar nunca una vez inicia la ejecución.
	/// </summary>
	IContext ProgramContext { get; }

	/// <summary>
	/// Una sesion que se usa para administrar los <see cref="IScope"/> y <see cref="IContext"/> relacionados al programa.
	/// </summary>
	IInterpreterSesion Sesion { get; }

	/// <summary>
	/// Inicia la ejecución del programa pasado.
	/// </summary>
	/// <param name="programContext">El programa kys parseado que se va a ejecutar</param>
	void Start(ProgramContext programContext);

	/// <summary>
	/// En caso de soportarlo, detiene la ejecución del programa.
	/// </summary>
	void Stop();
}
 