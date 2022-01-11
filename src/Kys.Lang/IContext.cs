namespace Kys.Lang;

public interface IContext
{
	/// <summary>
	/// <see cref="IScope"/> raiz de este contexto.
	/// </summary>
	IScope RootScope { get; }

	/// <summary>
	/// Lista de funciones que han sido definidas en este contexto.
	/// </summary>
	IEnumerable<IFunction> DefinedFunctions { get; }

	/// <summary>
	/// Agrega una función al contexto.
	/// </summary>
	/// <remarks>
	/// Este metodo no genera error simplemente devuelve un booleano con el resultado.
	/// </remarks>
	/// <param name="Function">Función a agregar.</param>
	/// <returns><c>true</c> si ha sido posible agregar la función, <c>false</c> si la función ya ha sido agregada o ya existe una con el mismo nombre.</returns>
	bool AddFunction(IFunction Function);

	/// <summary>
	/// Remueve una función de este contexto.
	/// </summary>
	/// <remarks>
	/// Este metodo no genera error simplemente devuelve un booleano con el resultado.
	/// </remarks>
	/// <param name="Name">Nombre de la función que quiere ser removida.</param>
	/// <returns><c>true</c> si la función pudo ser removida correctamente, <c>false</c> en caso contrario.</returns>
	bool RemoveFunction(string Name);

	/// <summary>
	/// Agrega o cambia una función existente. Si la función aun no existe la crea y si ya existe la cambia.
	/// </summary>
	/// <param name="Function">La función que quiere agregarse o modificarse.</param>
	void OverrideFunction(IFunction Function);

	/// <summary>
	/// Obtiene una función en este contexto.
	/// </summary>
	/// <param name="Name"></param>
	/// <returns></returns>
	IFunction GetFunction(string Name);

	/// <summary>
	/// Indica si este contexto puede ser ejecutado, si un contexto no puede ser ejecutado entonces solo puede ser usado para cargar funciones y almacenarlas pero no se podran ejecutar sentencias ni bloques asi como su <see cref="RootScope"/> no sera usado para almacenar variables.
	/// </summary>
	bool CanExecute { get; }
}
