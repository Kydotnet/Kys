namespace Kys.Interpreter.Visitors;

/// <summary>
/// Un visitor provee los metodos para analizar y ejecutar contextos sintacticos creados por antlr.
/// </summary>
/// <typeparam name="T">Tipo de valor devuelto por el visitor luego de analizar un contexto.</typeparam>
public interface IVisitor<T> : IKysParserVisitor<T>
{
	/// <summary>
	/// En caso de necesitar depencecias de otros <see cref="IVisitor{T}"/> se deben configurar en este metodo para evitar llamados recursivos, este metodo es llamado antes de cualquier uso, luego de la instanciación.
	/// </summary>
	/// <param name="serviceProvider">Resolutor de servicios del programa.</param>
	void Configure(IServiceProvider serviceProvider);
}

