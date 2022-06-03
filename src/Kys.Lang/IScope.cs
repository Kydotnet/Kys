using Kys.Runtime;

namespace Kys.Lang;

/// <summary>
/// Un Scope es un contenedor de variables asignado a un bloque de codigo, siempre tiene un <see cref="IScope.ParentScope"/> a menos que sea el root.
/// </summary>
public interface IScope
{

	/// <summary>
	/// El <see cref="IScope"/> padre al cual esta instancia tiene acceso a variables.
	/// </summary>
	IScope? ParentScope { get; set; }

	/// <summary>
	/// Limpia este scope eliminando todas las variables que contiene.
	/// </summary>
	void Clear();

	/// <summary>
	/// Indica si este scope contiene una definición para la variable dada.
	/// </summary>
	/// <param name="id">Identificador de la variable a buscar.</param>
	/// <param name="recursive">Indica si se debe ver si los <see cref="IScope"/> padres tambien contienen la variable.</param>
	/// <returns><c>true</c> si se tiene una variable <paramref name="id"/>, <c>false</c> si no se tiene.</returns>
	bool ConVar(string id, bool recursive = false);

	/// <summary>
	/// Asigna un valor a una variable existente, si la variable existe en este o en otro <see cref="IScope"/> padre entonces cambia su valor, si no existe genera un error.
	/// </summary>
	/// <remarks>
	/// Esta función es la que se ejecuta cuando en Kys se usa la sentencia:
	/// <code>ID = value;</code>
	/// El comportamiento descrito es el por defecto pero puede ser sobreescrito en algunos casos.
	/// </remarks>
	/// <param name="id">Identificador de la variable.</param>
	/// <param name="value">Valor a asignar a la variable.</param>
	/// <param name="recursive">Indica si se deben revisar los <see cref="IScope"/> padres de forma recursiva.</param>
	void AsigVar(string id, IKyObject value, bool recursive = true);

	/// <summary>
	/// Asigna a un valor a una variable o la crea si no existe, si la variable existe en este o en otro <see cref="IScope"/> padre entonces cambia su valor, si no existe crea una variable en este <see cref="IScope"/> y le asigna <paramref name="value"/>.
	/// </summary>
	/// <remarks>
	/// Esta función es la que se ejecuta cuando en Kys se usa la sentencia:
	/// <code>set ID = value;</code>
	/// El comportamiento descrito es el por defecto pero puede ser sobreescrito en algunos casos.
	/// </remarks>
	/// <param name="id">Identificador de la variable.</param>
	/// <param name="value">Valor a establecer en la variable.</param>
	/// <param name="recursive">Indica si se deben revisar los <see cref="IScope"/> padres de forma recursiva.</param>
	void SetVar(string id, IKyObject value, bool recursive = true);

	/// <summary>
	/// Crea una variable y le asigna un valor o no hace nada si la variable ya existe,  si la variable existe en este o en otro <see cref="IScope"/> padre entonces no hace nada, si no existe crea una variable en este <see cref="IScope"/> y le asigna <paramref name="value"/>.
	/// </summary>
	/// <remarks>
	/// Esta función es la que se ejecuta cuando en Kys se usa la sentencia:
	/// <code>def ID = value;</code>
	/// El comportamiento descrito es el por defecto pero puede ser sobreescrito en algunos casos.
	/// </remarks>
	/// <param name="id">Identificador de la variable.</param>
	/// <param name="value">Valor a definir en la variable.</param>
	/// <param name="recursive">Indica si se deben revisar los <see cref="IScope"/> padres de forma recursiva.</param>
	void DefVar(string id, IKyObject value, bool recursive = true);

	/// <summary>
	/// Crea una variable y le asigna un valore,  si la variable existe en este o en otro <see cref="IScope"/> padre entonces se genera un error, si no existe crea una variable en este <see cref="IScope"/> y le asigna <paramref name="value"/>.
	/// </summary>
	/// <remarks>
	/// Esta función es la que se ejecuta cuando en Kys se usa la sentencia:
	/// <code>var ID = value;</code>
	/// El comportamiento descrito es el por defecto pero puede ser sobreescrito en algunos casos.
	/// </remarks>
	/// <param name="id">Identificador de la variable.</param>
	/// <param name="value">Valor a declarar en la variable.</param>
	/// <param name="recursive">Indica si se deben revisar los <see cref="IScope"/> padres de forma recursiva.</param>
	void DecVar(string id, IKyObject value, bool recursive = true);

	/// <summary>
	/// Obtiene el valor de una variable en este o en un <see cref="IScope"/> padre, si la variable no existe se genera un error.
	/// </summary>
	/// <param name="id">Identificador de la variable.</param>
	/// <returns>Valor almacenado en la variable</returns>
	/// <param name="recursive">Indica si se deben revisar los <see cref="IScope"/> padres de forma recursiva.</param>
	IKyObject GetVar(string id, bool recursive = true);
}
