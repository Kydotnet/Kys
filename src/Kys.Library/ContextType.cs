namespace Kys.Library;

/// <summary>
/// Tipos de <see cref="IContext"/> que se pueden crear.
/// </summary>
[Flags]
public enum ContextType
{
	/// <summary>
	/// Uso generico.
	/// </summary>
	All = 0b0,
	/// <summary>
	/// Uso en el contexto principal del programa.
	/// </summary>
	Me = 0b1,
	/// <summary>
	/// Uso en contextos creados por instrucciones Kys.
	/// </summary>
	Kys = 0b11,
	/// <summary>
	/// Uso en contextos creados por instrucciones Kyl.
	/// </summary>
	Package = 0b101
}