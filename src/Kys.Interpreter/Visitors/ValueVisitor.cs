using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Antlr4.Runtime.Tree;
using Kys.Parser.Extensions;
using Kys.Runtime;
#pragma warning disable CS8764
namespace Kys.Interpreter.Visitors;

/// <summary>
/// Implementaci�n por defecto de <see cref="IVisitor{T}"/> para ejecutar <see cref="ValueContext"/> y <see cref="FuncresultContext"/>.
/// </summary>
public class ValueVisitor : BaseVisitor<IKyObject>
{
	#pragma warning disable CS8618
	IKysParserVisitor<IKyObject> _expressionVisitor;
	#pragma warning restore CS8618

	/// <inheritdoc/>
	public override void Configure(IServiceProvider serviceProvider)
	{
		base.Configure(serviceProvider);
		_expressionVisitor = VisitorProvider.GetVisitor<ExpressionContext>();
	}

	/// <summary>
	/// Busca en el contexto actual una función con nombre <see cref="FuncresultContext.ID()"/> y la llama.
	/// </summary>
	/// <inheritdoc/>
	/// <returns>Devuelve lo mismo devuelto por la función.</returns>
	public override IKyObject VisitFuncresult([Antlr4.Runtime.Misc.NotNull] FuncresultContext context)
	{
		var funcname = context.ID().GetCacheText();
		Sesion["LastToken"] = context.ID().Symbol;

		var func = Sesion.CurrentContext.GetFunction(funcname);

		var funcargs = context.arguments();
		var hasargs = funcargs != null;

		IKyObject[] args = hasargs ? funcargs!.expression().Select(_expressionVisitor.Visit).ToArray() : Array.Empty<IKyObject>();
		var scope = Sesion.StartScope(ScopeType.Function);

		//TODO: producir error cuando no existe la función;
		var dev = func?.Call(Sesion.CurrentContext, scope, args);

		Sesion.EndScope();
		// si se devuelve algo que no sea nulo el InstructionVisitor terminara
		return dev;
	}

	/// <summary>
	/// Interpreta el valor del <see cref="ValueContext"/> y lo devuelve.
	/// </summary>
	/// <inheritdoc/>
	public override IKyObject VisitValue([Antlr4.Runtime.Misc.NotNull] ValueContext context)
	{
		if (context.String is ITerminalNode String)
			return GetString(String);
		if (context.Number is ITerminalNode Number)
			return GetNumber(Number);
		if (context.Bool is ITerminalNode Bool)
			return GetBool(Bool);
		if (context.Id is ITerminalNode Id)
			return GetVar(Sesion, Id);

		return Null;
	}
	  
	/// <summary>
	/// Obtiene un valor booleano a partir de un <see cref="KysLexer.BOOL"/>.
	/// </summary>
	/// <param name="terminalNode">El nodo que quiere ser convertido en booleando.</param>
	/// <returns>Devuelve el valor obtenido, <c>true</c> o <c>false</c>.</returns>
	public static IKyObject GetBool(ITerminalNode terminalNode)
	{
		var raw = terminalNode.GetCacheText().ToLower();
		return raw.Equals("true") ? True : False;
	}

	/// <summary>
	/// Obtiene el valor de una varible de nombre <see cref="KysLexer.ID"/> desde una sesi�n <paramref name="sesion"/>.
	/// </summary>
	/// <param name="sesion">La sesi�n desde la cual se obtendra la variable.</param>
	/// <param name="terminalNode">El nombre de la variable a obtener.</param>
	/// <returns>Devuelve el valor obtenido desde la sesi�n o propaga el error en caso de no estar definida.</returns>
	public static IKyObject GetVar(IInterpreterSesion sesion, ITerminalNode terminalNode)
	{
		var raw = terminalNode.GetCacheText();

		return sesion.CurrentScope.GetVar(raw);
	}


	/// <summary>
	/// Obtiene un <see cref="int"/> o un <see cref="double"/> que se obtiene al parsear un <see cref="KysLexer.NUMBER"/>.
	/// </summary>
	/// <param name="terminalNode">El nodo que queire ser convertido en numero.</param>
	/// <returns>Devuelve el numero obtenido, ya se un entero o un numero de doble presici�n.</returns>
	[SuppressMessage("Style", "IDE0046:Convertir a expresi�n condicional", Justification = "Reducir el if produce que un int se retorne como double, lo que genera error en ejecuci�n")]
	public static IKyObject GetNumber(ITerminalNode terminalNode)
	{
		var raw = terminalNode.GetCacheText();
		if (int.TryParse(raw, out int retint))
			return FromValue(retint);

		return  FromValue(double.Parse(raw));
	}

	internal static int GetInt(ITerminalNode terminalNode)
	{
		return int.Parse(terminalNode.GetCacheText(), NumberStyles.Integer, null);
	}

	/// <summary>
	/// Obtiene una cadena de texto desde un <see cref="ITerminalNode"/> de representa un item <see cref="KysLexer.STRING"/>.
	/// </summary>
	/// <param name="terminalNode">El nodo que quiere se convertido en string.</param>
	/// <returns>Devuelve el texto contenido en el <see cref="KysLexer.STRING"/>.</returns>
	public static IKyObject GetString(ITerminalNode terminalNode)
	{
		var raw = terminalNode.GetCacheText();
		return FromValue(raw.Trim('"'));
	}
}
