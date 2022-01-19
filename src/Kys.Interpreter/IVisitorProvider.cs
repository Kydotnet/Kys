using Antlr4.Runtime;
using Kys.Interpreter.Visitors;
using Kys.Parser;

namespace Kys.Interpreter;

/// <summary>
/// Provee una forma de administrar los visitor que se usan en un programa de Kys.
/// </summary>
public interface IVisitorProvider
{
	/// <summary>
	/// Agrega un tipo que se usara como visitor para un contexto especifico.
	/// </summary>
	/// <typeparam name="VisitorContext">El contexto para el cual puede ser usado este visitor.</typeparam>
	/// <typeparam name="Implementation">El tipo del visitor que debe ser instanciado.</typeparam>
	void AddVisitor<VisitorContext, Implementation>() where VisitorContext : ParserRuleContext where Implementation : IVisitor<object>;

	/// <summary>
	/// Agrega un visitor ya instanciado para usar en un contexto especifico.
	/// </summary>
	/// <typeparam name="VisitorContext">El contexto para el cual puede ser usado este visitor.</typeparam>
	/// <param name="implementation">Una instancia de un visitor que sera usada, ya debe estar configurado y listo para suar.</param>
	void AddVisitor<VisitorContext>(IVisitor<object> implementation) where VisitorContext : ParserRuleContext;

	/// <summary>
	/// Obtiene un visitor para un contexto especifico.
	/// </summary>
	/// <typeparam name="VisitorContext">El contexto para el cual sera usado el visitor.</typeparam>
	/// <returns>Una instancia de un visitor listo para usar en el conetxto.</returns>
	IVisitor<object> GetVisitor<VisitorContext>() where VisitorContext : ParserRuleContext;
}
 