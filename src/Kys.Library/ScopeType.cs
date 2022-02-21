namespace Kys.Library;

/// <summary>
/// Tipos de <see cref="IScope"/> que se pueden crear.
/// </summary>
public enum ScopeType
{
	/// <summary>
	/// Scope para cualquier uso.
	/// </summary>
	All = 0b0,
	/// <summary>
	/// Scope usado para el scope que se usa en el <see cref="IContext"/> principal del programa.
	/// </summary>
	Me = 0b1,
	/// <summary>
	/// Scope que se usa para contextos creados por Kys.
	/// </summary>
	Kys = 0b101,
	/// <summary>
	/// Scope que se usa en contexto creados por Kyl.
	/// </summary>
	Package = 0b1001,
	/// <summary>
	/// Scope que se usa al llamar a una función.
	/// </summary>
	Function = 0b10,
	/// <summary>
	/// Scope que se usa cuando se entra en una estructura de control.
	/// </summary>
	Control = 0b110,
}
