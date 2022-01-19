namespace Kys.Library;

/// <summary>
/// Tipos de <see cref="IScope"/> que se pueden crear.
/// </summary>
public enum ScopeType
{
	/// <summary>
	/// Scope para cualquier uso.
	/// </summary>
	ALL = 0b0,
	/// <summary>
	/// Scope usado para el scope que se usa en el <see cref="IContext"/> principal del programa.
	/// </summary>
	ME = 0b1,
	/// <summary>
	/// Scope que se usa para contextos creados por Kys.
	/// </summary>
	KYS = 0b101,
	/// <summary>
	/// Scope que se usa en contexto creados por Kyl.
	/// </summary>
	PACKAGE = 0b1001,
	/// <summary>
	/// Scope que se usa al llamar a una función.
	/// </summary>
	FUNCTION = 0b10,
	/// <summary>
	/// Scope que se usa cuando se entra en una estructura de control.
	/// </summary>
	CONTROL = 0b110,
}
