using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System.Globalization;

namespace Kys.Interpreter.Visitors;

/// <summary>
/// Implementación por defecto de <see cref="IVisitor{T}"/> para ejecutar <see cref="ValueContext"/> y <see cref="FuncresultContext"/>.
/// </summary>
public class ValueVisitor : BaseVisitor<dynamic>
{
	IKysParserVisitor<dynamic> expressionVisitor;

	/// <inheritdoc/>
	public override void Configure(IServiceProvider serviceProvider)
	{
		base.Configure(serviceProvider);
		expressionVisitor = VisitorProvider.GetVisitor<ExpressionContext>();
	}

	/// <summary>
	/// Busca en el contexto actual una función con nombre <see cref="FuncresultContext.ID()"/> y la llama.
	/// </summary>
	/// <inheritdoc/>
	/// <returns>Devuelve lo mismo devuelto por la función.</returns>
	public override dynamic VisitFuncresult([NotNull] FuncresultContext context)
	{
		var funcname = context.ID().GetText();
		Sesion["LastToken"] = context.ID().Symbol;

		var func = Sesion.CurrentContext.GetFunction(funcname);

		var funcargs = context.arguments();
		var hasargs = funcargs != null;

		dynamic[] args = hasargs ? funcargs.expression().Select(v => expressionVisitor.Visit(v)).ToArray() : Array.Empty<dynamic>();
		var scope = Sesion.StartScope(ScopeType.FUNCTION);

		//TODO: producir error cuando no existe la función;
		var dev = func.Call(Sesion.CurrentContext, scope, args);

		Sesion.EndScope();
		// si se devuelve algo que no sea nulo el InstructionVisitor terminara
		return dev;
	}

	/// <summary>
	/// Interpreta el valor del <see cref="ValueContext"/> y lo devuelve.
	/// </summary>
	/// <inheritdoc/>
	public override dynamic VisitValue([NotNull] ValueContext context)
	{
		if (context.STRING() != null)
			return GetString(context.STRING());
		else if (context.NUMBER() != null)
			return GetNumber(context.NUMBER());
		else if (context.BOOL() != null)
			return GetBool(context.BOOL());
		else if (context.ID() != null)
			return GetVar(Sesion, context.ID());

		return null;
	}

	/// <summary>
	/// Obtiene un valor booleano a partir de un <see cref="KysLexer.BOOL"/>.
	/// </summary>
	/// <param name="terminalNode">El nodo que quiere ser convertido en booleando.</param>
	/// <returns>Devuelve el valor obtenido, <c>true</c> o <c>false</c>.</returns>
	public static bool GetBool(ITerminalNode terminalNode)
	{
		var raw = terminalNode.GetText().ToLower();
		return raw.Equals("true");
	}

	/// <summary>
	/// Obtiene el valor de una varible de nombre <see cref="KysLexer.ID"/> desde una sesión <paramref name="sesion"/>.
	/// </summary>
	/// <param name="sesion">La sesión desde la cual se obtendra la variable.</param>
	/// <param name="terminalNode">El nombre de la variable a obtener.</param>
	/// <returns>Devuelve el valor obtenido desde la sesión o propaga el error en caso de no estar definida.</returns>
	public static dynamic GetVar(IInterpreterSesion sesion, ITerminalNode terminalNode)
	{
		var raw = terminalNode.GetText();

		return sesion.CurrentScope.GetVar(raw);
	}
	
	/// <summary>
	/// Obtiene un <see cref="int"/> o un <see cref="double"/> que se obtiene al parsear un <see cref="KysLexer.NUMBER"/>.
	/// </summary>
	/// <param name="terminalNode">El nodo que queire ser convertido en numero.</param>
	/// <returns>Devuelve el numero obtenido, ya se un entero o un numero de doble presición.</returns>
	public static dynamic GetNumber(ITerminalNode terminalNode)
	{
		var raw = terminalNode.GetText();
		return int.TryParse(raw, out int retint) ? retint : double.Parse(raw, CultureInfo.InvariantCulture);
	}

	/// <summary>
	/// Obtiene una cadena de texto desde un <see cref="ITerminalNode"/> de representa un item <see cref="KysLexer.STRING"/>.
	/// </summary>
	/// <param name="terminalNode">El nodo que quiere se convertido en string.</param>
	/// <returns>Devuelve el texto contenido en el <see cref="KysLexer.STRING"/>.</returns>
	public static string GetString(ITerminalNode terminalNode)
	{
		var raw = terminalNode.GetText();
		return raw.Trim('"');
	}
}
