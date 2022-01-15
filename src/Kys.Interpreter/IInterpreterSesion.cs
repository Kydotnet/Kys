namespace Kys.Interpreter;
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

	IScope StartScope(ScopeFactoryType type);

	IScope EndScope();
}
