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
	/// Almacena el indice de la ultima linea que se ejecuto en el programa, solo si el interprete lo soporta.
	/// </summary>
	int LastLine { get; set; }

	
	IScope StartScope(ScopeFactoryType type);

	IScope EndScope();
}
