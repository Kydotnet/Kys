namespace Kys.Library;

/// <summary>
/// Provee un generador de objetos <see cref="IContext"/>.
/// </summary>
public interface IContextFactory
{
	/// <summary>
	/// Cambia el tipo de contexto que sera usado cuando se quiera generar un contexto para <paramref name="type"/>.
	/// </summary>
	/// <typeparam name="T">Tipo que sera instanciado cada vez que se requiera un contexto de <paramref name="type"/>.</typeparam>
	/// <param name="type">Tipo de contexto que sera reemplazado.</param>
	void ChangeContext<T>(ContextType type) where T : IContext;

	/// <summary>
	/// Genera un <see cref="IContext"/> con un proposito dado.
	/// </summary>
	/// <param name="type">El tipo de contexto que se debe generar, es decir, el proposito.</param>
	/// <returns>Devuelve el contexto generado.</returns>
	IContext Create(ContextType type);
}