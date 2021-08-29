namespace Kys.Lang;

/// <summary>
/// Un Scope es un contenedor de variables asignado a un bloque de codigo, siempre tiene un <see cref="IScope.ParentScope"/> a menos que sea el root.
/// </summary>
public interface IScope
{
	/// <summary>
	/// Lista de Scopes iniciados.
	/// </summary>
	private static readonly Stack<IScope> ScopeStack = new();

	/// <summary>
	/// El Scope que se esta usando actualmente.
	/// </summary>
	public static IScope Current => ScopeStack.Peek();

	/// <summary>
	/// Busca entre una jerarquia de <see cref="IScope"/> al primero que contenga una definición para <paramref name="ID"/>.
	/// </summary>
	/// <param name="ID">Identificador de la variable a buscar.</param>
	/// <param name="Start">Scope por el cual se empieza la busqueda, luego se vera su padre y luego el padre del padre y asi sucesivamente.</param>
	/// <returns>Devuelve el primer scope que se encuentre con una definición para <paramref name="ID"/>, si no se encuentra ninguno se devuelve <c>null</c>.</returns>
	public static IScope CheckRecursive(string ID, IScope Start)
	{
		if (Start.ConVar(ID))
			return Start;
		if (Start.ParentScope != null)
			return CheckRecursive(ID, Start.ParentScope);
		return null;
	}

	/// <summary>
	/// El <see cref="IScope"/> padre al cual esta instancia tiene acceso a variables. Esta propiedad no puede ser cambiado luego de inicializada la variable.
	/// </summary>
	IScope ParentScope { get; init; }

	/// <summary>
	/// Indica que se ha dejadod e trabajar en este scope por lo que se debe vovler al anterior.
	/// </summary>
	bool Stop()
	{
		if (this == Current)
		{
			ScopeStack.Pop().Clear();
			return true;
		}
		return false;
	}

	/// <summary>
	/// Indica que ahra se esta trabajando en este scope.
	/// </summary>
	void Start() => ScopeStack.Push(this);

	/// <summary>
	/// Limipa este scope eliminando todas las variables que contiene.
	/// </summary>
	void Clear();

	/// <summary>
	/// Indica si este scope contiene una definición para la variable dada.
	/// </summary>
	/// <remarks>
	/// Este metodo no es recursivo, para buscar recursivamente se usa <see cref="CheckRecursive(string, IScope)"/>.
	/// </remarks>
	/// <param name="ID">Identificador de la variable a buscar.</param>
	/// <returns><c>true</c> si se tiene una variable <paramref name="ID"/>, <c>false</c> si no se tiene.</returns>
	bool ConVar(string ID);

	/// <summary>
	/// Asigna un valor a una variable existente, si la variable existe en este o en otro <see cref="IScope"/> padre entonces cambia su valor, si no existe genera un error.
	/// </summary>
	/// <remarks>
	/// Esta función es la que se ejecuta cuando en Kys se usa la sentencia:
	/// <code>ID = value;</code>
	/// El comportamiento descrito es el por defecto pero puede ser sobreescrito en algunos casos.
	/// </remarks>
	/// <param name="ID">Identificador de la variable.</param>
	/// <param name="value">Valor a asignar a la variable.</param>
	void AsigVar(string ID, dynamic value, bool recursive = true);

	/// <summary>
	/// Asigna a un valor a una variable o la crea si no existe, si la variable existe en este o en otro <see cref="IScope"/> padre entonces cambia su valor, si no existe crea una variable en este <see cref="IScope"/> y le asigna <paramref name="value"/>.
	/// </summary>
	/// <remarks>
	/// Esta función es la que se ejecuta cuando en Kys se usa la sentencia:
	/// <code>set ID = value;</code>
	/// El comportamiento descrito es el por defecto pero puede ser sobreescrito en algunos casos.
	/// </remarks>
	/// <param name="ID">Identificador de la variable.</param>
	/// <param name="value">Valor a establecer en la variable.</param>
	void SetVar(string ID, dynamic value, bool recursive = true);

	/// <summary>
	/// Crea una variable y le asigna un valor o no hace nada si la variable ya existe,  si la variable existe en este o en otro <see cref="IScope"/> padre entonces no hace nada, si no existe crea una variable en este <see cref="IScope"/> y le asigna <paramref name="value"/>.
	/// </summary>
	/// <remarks>
	/// Esta función es la que se ejecuta cuando en Kys se usa la sentencia:
	/// <code>def ID = value;</code>
	/// El comportamiento descrito es el por defecto pero puede ser sobreescrito en algunos casos.
	/// </remarks>
	/// <param name="ID">Identificador de la variable.</param>
	/// <param name="value">Valor a definir en la variable.</param>
	void DefVar(string ID, dynamic value, bool recursive = true);

	/// <summary>
	/// Crea una variable y le asigna un valore,  si la variable existe en este o en otro <see cref="IScope"/> padre entonces se genera un error, si no existe crea una variable en este <see cref="IScope"/> y le asigna <paramref name="value"/>.
	/// </summary>
	/// <remarks>
	/// Esta función es la que se ejecuta cuando en Kys se usa la sentencia:
	/// <code>var ID = value;</code>
	/// El comportamiento descrito es el por defecto pero puede ser sobreescrito en algunos casos.
	/// </remarks>
	/// <param name="ID">Identificador de la variable.</param>
	/// <param name="value">Valor a declarar en la variable.</param>
	void DecVar(string ID, dynamic value, bool recursive = true);

	/// <summary>
	/// Obtiene el valor de una variable en este o en un <see cref="IScope"/> padre, si la variable no existe se genera un error.
	/// </summary>
	/// <param name="ID">Identificador de la variable.</param>
	/// <returns>Valor almacenado en la variable</returns>
	dynamic GetVar(string ID, bool recursive = true);
}
