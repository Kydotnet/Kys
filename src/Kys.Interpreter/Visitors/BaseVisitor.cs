using Microsoft.Extensions.DependencyInjection;
namespace Kys.Interpreter.Visitors;

/// <summary>
/// Implementacion base de <see cref="IVisitor{T}"/>, esta implementacion no hace nada por si sola y debe ser heredada.
/// </summary>
/// <typeparam name="T"></typeparam>
public class BaseVisitor<T> : KysParserBaseVisitor<T>, IVisitor<T>
{
	/// <summary>
	/// Sesion del interprete actual.
	/// </summary>
	protected IInterpreterSesion Sesion = null!;

	/// <summary>
	/// <see cref="IVisitorProvider"/> que se puede usar para obtener visitor evitando referencias circulares.
	/// </summary>
	protected IVisitorProvider VisitorProvider = null!;

	/// <summary>
	/// Este metodo es lllamado cuando se requiere configurar servicios o visitor de forma que se evita la creación recursiva.
	/// </summary>
	/// <param name="serviceProvider"><see cref="IServiceProvider"/> que contiene los servicios disponibles a utilizar.</param>
	public virtual void Configure(IServiceProvider serviceProvider)
	{
		Sesion = serviceProvider.GetRequiredService<IInterpreterSesion>();
		VisitorProvider = serviceProvider.GetRequiredService<IVisitorProvider>();
	}
}
