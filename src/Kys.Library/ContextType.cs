namespace Kys.Library;

/// <summary>
/// Tipos de <see cref="IContext"/> que se pueden crear.
/// </summary>
public enum ContextType
{
	/// <summary>
	/// Uso generico.
	/// </summary>
	ALL = 0b0,
	/// <summary>
	/// Uso en el contexto principal del programa.
	/// </summary>
	ME = 0b1,
	/// <summary>
	/// Uso en contextos creados por instrucciones Kys.
	/// </summary>
	KYS = 0b11,
	/// <summary>
	/// Uso en contextos creados por instrucciones Kyl.
	/// </summary>
	PACKAGE = 0b101
}