namespace Kys.Lang;

public interface IContext
{
	/// <summary>
	/// Este es el contexto principal del programa que representa el archivo de Kys cargado.
	/// </summary>
	public static IContext Me { get; internal set; } = new RuntimeContext();

	/// <summary>
	/// Cambia el contexto a otra instancia de <see cref="IContext"/>. El cmabio de contexto solo esta permitido si aun no se ha iniciado la ejecución, de lo contrario se generara una excepción.
	/// </summary>
	/// <param name="context">Nuevo contexto a aplicar.</param>
	public static void ChangeContext(IContext context)
	{
		if (Me.IsStarted)
			throw new InvalidOperationException("No se puede cambiar el contexto una vez que ha iniciado la ejecución del actual.");
		if (context.CanExecute)
			Me = context;
	}

	/// <summary>
	/// Inicia la ejecución del contexto <see cref="Me"/>, lo cual a su vez inicializa su propiedad <see cref="RootScope"/>.
	/// </summary>
	public static void Start()
	{
		Me.IsStarted = true;
		Me.RootScope.Start();
	}

	/// <summary>
	/// Detiene la ejecución del contexto lo cal intenta cerrar a <see cref="RootScope"/>.
	/// </summary>
	/// <returns><c>true</c> si se puedo terminar correctamente la ejecución y cerrar <see cref="RootScope"/>, <c>false</c> en caso contrario.</returns>
	public static bool Stop()
	{
		if (Me.RootScope.Stop())
		{
			Me.IsStarted = false;
			return true;
		}
		return false;
	}

	/// <summary>
	/// <see cref="IScope"/> raiz de este contexto.
	/// </summary>
	IScope RootScope { get; init; }

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
	/// A diferencia del metodo <see cref="AddFunction(IFunction)"/> este no puede ser llamado si este contexto esta en ejecución o no ocurrira nada.
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

	/// <summary>
	/// Indica si este contexto esta iniciado.
	/// </summary>
	bool IsStarted { get; internal set; }
}
