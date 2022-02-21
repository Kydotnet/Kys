namespace Kys.Interpreter;

/// <summary>
/// Una sesión contiene los <see cref="IScope"/> del programa asi como tambien guarda información sobre la ejecución del mismo.
/// </summary>
public interface IInterpreterSesion
{
	/// <summary>
	/// Obtiene el interprete al que esta ligado esta sesión.
	/// </summary>
	/// <remarks>
	///Esta propiedad debe ser inicializada por el Host.
	/// </remarks>
	IInterpreter Interpreter { get; set; }

	/// <summary>
	/// El contexto en el que nos encontramos en este momento.
	/// </summary>
	/// <remarks>
	/// Esta propiedad debe ser inicializada por el interprete.
	/// </remarks>
	IContext CurrentContext { get; set; }

	/// <summary>
	/// El scope actualmente activo.
	/// </summary>
	/// <remarks>
	/// Esta propiedad debe ser inicializada por el interprete.
	/// </remarks>
	IScope CurrentScope { get; set; }

	/// <summary>
	/// En caso de trabajar con varios contextos, este hace referencia al contexto que invoco al contexto actual.
	/// </summary>
	IContext? CallerContext { get; set; }

	/// <summary>
	/// Almacena o obtiene una variable de sesion.
	/// </summary>
	/// <param name="name"></param>
	/// <returns></returns>
	object? this[string name] { get; set; }

	/// <summary>
	/// Inicia un nuevo scope para un tipo especifico.
	/// </summary>
	/// <param name="type">El tipo para el cual sera usado ese scope.</param>
	/// <returns></returns>
	IScope StartScope(ScopeType type);

	/// <summary>
	/// Finaliza el <see cref="IScope"/> que esta siendo usado y vuelve al anterior.
	/// </summary>
	/// <returns>Devuelve una referencia al scope que se a finalizado, aunque estara vacio ya que es limpiado al finalizarse.</returns>
	void EndScope();
}
