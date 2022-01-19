namespace Kys.Interpreter;

/// <summary>
/// Una sesión contiene los <see cref="IScope"/> del programa asi como tambien guarda información sobre la ejecución del mismo.
/// </summary>
public interface IInterpreterSesion
{
	/// <summary>
	/// El contexto en el que nos encontramos en este momento.
	/// </summary>
	IContext CurrentContext { get; set; }

	/// <summary>
	/// El scope actualemnte activo.
	/// </summary>
	IScope CurrentScope { get; set; }

	/// <summary>
	/// En caso de trabajar con varios contextos, este hace referencia al contexto que llamo la función actual.
	/// </summary>
	IContext CallerContext { get; set; }

	/// <summary>
	/// Almacena o obtiene una variable de sesion.
	/// </summary>
	/// <param name="name"></param>
	/// <returns></returns>
	object this[string name] { get; set; }

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
	IScope EndScope();
}
