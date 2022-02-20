namespace Kys.Library;


/// <summary>
/// Provee un generador de objetos <see cref="IScope"/>.
/// </summary>
public interface IScopeFactory
{
	/// <summary>
	/// Cambia el tipo de scope que sera usado cuando se quiera generar un scope para <paramref name="type"/>.
	/// </summary>
	/// <typeparam name="T">Tipo que sera instanciado cada vez que se requiera un scope de <paramref name="type"/>.</typeparam>
	/// <param name="type">Tipo de scope que sera reemplazado.</param>
	void ChangeScope<T>(ScopeType type) where T : IScope;

	/// <summary>
	/// Genera un <see cref="IScope"/> con un proposito dado.
	/// </summary>
	/// <param name="type">El tipo de scope que se debe generar, es decir, el proposito.</param>
	/// <param name="parent">Un scope opcional que sera el padre del scope generado. Ver <seealso cref="IScope.ParentScope"/></param>
	/// <returns>Devuelve el scope generado.</returns>
	IScope Create(ScopeType type, IScope parent = null);
}
