using Antlr4.Runtime;
using Kys.Interpreter.Visitors;

namespace Kys.Interpreter;

/// <summary>
/// Provee una forma de administrar los visitor que se usan en un programa de Kys.
/// </summary>
public interface IVisitorProvider
{
	/// <summary>
	/// Agrega un tipo que se usara como visitor para un contexto especifico.
	/// </summary>
	/// <typeparam name="TVisitorContext">El contexto para el cual puede ser usado este visitor.</typeparam>
	/// <typeparam name="TImplementation">El tipo del visitor que debe ser instanciado.</typeparam>
	void AddVisitor<TVisitorContext, TImplementation>() where TVisitorContext : ParserRuleContext where TImplementation : IVisitor<object>;

	/// <summary>
	/// Agrega un visitor ya instanciado para usar en un contexto especifico.
	/// </summary>
	/// <typeparam name="TVisitorContext">El contexto para el cual puede ser usado este visitor.</typeparam>
	/// <param name="implementation">Una instancia de un visitor que sera usada, ya debe estar configurado y listo para suar.</param>
	void AddVisitor<TVisitorContext>(IVisitor<object> implementation) where TVisitorContext : ParserRuleContext;

	/// <summary>
	/// Obtiene un visitor para un contexto especifico.
	/// </summary>
	/// <typeparam name="TVisitorContext">El contexto para el cual sera usado el visitor.</typeparam>
	/// <returns>Una instancia de un visitor listo para usar en el conetxto.</returns>
	IVisitor<object> GetVisitor<TVisitorContext>() where TVisitorContext : ParserRuleContext;
}
 