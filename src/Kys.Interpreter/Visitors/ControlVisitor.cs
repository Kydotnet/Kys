using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System.Threading;
using System.Threading.Tasks;

namespace Kys.Interpreter.Visitors;

/// <summary>
/// Implementación por defecto de <see cref="IVisitor{T}"/> para ejecutar <see cref="ControlContext"/>.
/// </summary>
public class ControlVisitor : BaseVisitor<object>
{
	IKysParserVisitor<dynamic> expressionVisitor;
	IKysParserVisitor<object> sentenceVisitor;
	IKysParserVisitor<object> varoperationVisitor;

	/// <inheritdoc/>
	public override void Configure(IServiceProvider serviceProvider)
	{
		base.Configure(serviceProvider);
		expressionVisitor = VisitorProvider.GetVisitor<ExpressionContext>();
		sentenceVisitor = VisitorProvider.GetVisitor<SentenceContext>();
		varoperationVisitor = VisitorProvider.GetVisitor<VaroperationContext>();
	}

	/// <summary>
	/// Por defecto se evalua <see cref="IfcontrolContext.expression()"/> usando el <see cref="IVisitor{T}"/> para el contexto <see cref="ExpressionContext"/>, si es <c>true</c> se ejecuta <see cref="IfcontrolContext.block()"/> con <see cref="VisitBlock(BlockContext)"/>, de lo contrario en caso de que <see cref="IfcontrolContext.elsecontrol()"/> es distinto de <c>null</c> se eejcuta con <see cref="VisitElsecontrol(ElsecontrolContext)"/>.
	/// </summary>
	/// <inheritdoc/>
	public override object VisitIfcontrol([NotNull] IfcontrolContext context)
	{
		Sesion.StartScope(ScopeType.CONTROL);

		if (Expistrue(context.expression()))
			VisitBlock(context.block());
		else if (context.elsecontrol() != null)
			VisitElsecontrol(context.elsecontrol());
		Sesion.EndScope();
		return null;
	}

	/// <summary>
	/// Por defecto si se trata de un "else if" encadenado lo ejecuta con <see cref="VisitIfcontrol(IfcontrolContext)"/>, de lo contrario ejecuta el contenido del bloque "else" con <see cref="VisitBlock(BlockContext)"/>.
	/// </summary>
	/// <inheritdoc/>
	public override object VisitElsecontrol([NotNull] ElsecontrolContext context)
	{
		if (context.ifcontrol() != null)
			VisitIfcontrol(context.ifcontrol());
		else
			VisitBlock(context.block());
		return null;
	}

	/// <summary>
	///  Por defecto se evalua <see cref="WhilecontrolContext.expression()"/> usando el <see cref="IVisitor{T}"/> para el contexto <see cref="ExpressionContext"/>, si es <c>true</c> se ejecuta <see cref="WhilecontrolContext.block()"/> con <see cref="VisitBlock(BlockContext)"/> y se vuelve a evaluar hasta que sea <c>false</c>.
	/// </summary>
	/// <inheritdoc/>
	public override object VisitWhilecontrol([NotNull] WhilecontrolContext context)
	{
		Sesion.StartScope(ScopeType.CONTROL);
		var block = context.block();
		var exp = context.expression();
		while (Expistrue(exp))
			VisitBlock(block);
		Sesion.EndScope();
		return null;
	}

	/// <summary>
	/// La estructura "timed while" es una estructura de control personalizada de kys y su funcionamiento es similar  a un while normal solo que permite proporcionar un tiempo en el cual la expresion es evaluada luego de terminar la ejecución anterior.
	/// </summary>
	/// <remarks>
	/// El bloque "timed while" s ele puede anexar un bloque "timeout" que especifica un valor maximo de tiempo a esperar el bucle, es decir, si se especifica un valor de <c>5000</c> entonces el bucle solo se ejecutara 5 segundos maximo, llegando solo a este limite si se ha evaluado como <c>true</c> siempre.
	/// El tiempo maximo del timeout es superado unicamente si la tarea interna de metodo tarda mas que el tiempo dado en timeout, por ejemplo, se da un tiempo de <c>100</c> pero ejecutar ese bloque toma 700ms, esto no interrumpira la ejecución del bloque si no que esperara a que finalize y ya no volvera a evaluar la expresion si no que ejecutara el bloque interno del timeout.
	/// </remarks>
	/// <inheritdoc/>
	public override object VisitTwhilecontrol([NotNull] TwhilecontrolContext context)
	{
		Sesion.StartScope(ScopeType.CONTROL);
		var info = context.twbucle();
		var block = info.block();
		var timed = info.timeoutcontrol();
		int wait = ValueVisitor.GetInt(info.NUMBER());
		var exp = info.expression();

		if (timed != null)
		{
			TimeoutWhile(block, timed, wait, exp);
		}
		else
		{
			while (Expistrue(exp))
			{
				VisitBlock(block);
				Task.Delay(wait).Wait();
			}
		}
		Sesion.EndScope();
		return null;
	}

	private void TimeoutWhile(BlockContext block, TimeoutcontrolContext timed, int wait, ExpressionContext exp)
	{
		var twait = ValueVisitor.GetInt(timed.NUMBER());
		var tblock = timed.block();
		var token = new CancellationTokenSource();

		// tarea de ejecución del bloque while
		var whiletask = InternalTWile(exp, token, wait, block);
		// tarea de esperar el timeot
		var waittask = Task.Delay(twait, token.Token);
		// esperamos que se termine el while o a que se acabe el timeout
		var t = Task.WhenAny(whiletask, waittask).Result;
		// cancelamos el token para que se pare el timeout o para que ya el while no itere mas
		token.Cancel();

		if(t == waittask)
		{
			// si acaba primero el timeout entonces primero esperamos a que acabe la ejecución del bucle while actual(en caso de ser pesado puede tardar mas que el tiempo de timeout)
			whiletask.Wait();
			VisitBlock(tblock);

		}// si el que acabo primero fue el whiletask entonces es porque acabo antes que el timeout por loque no se debe hacer nada mas.

	}

	private async Task InternalTWile(ExpressionContext exp, CancellationTokenSource token, int wait, BlockContext block)
	{
		while (Expistrue(exp) && !token.IsCancellationRequested)
		{
			VisitBlock(block);
			if (wait > 0)
				await Task.Delay(wait);
		}
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0046:Convertir a expresión condicional", Justification = "Convertir en retorno directo ocasiona que se use el callsite equivocado al compilar.")]
	private bool Expistrue(ExpressionContext exp)
	{
		if (expressionVisitor.Visit(exp)) return true;
		return false;
	}

	/// <summary>
	/// El comportotamiento por defecto de la estructura for es:
	/// </summary>
	/// <remarks>
	/// <list type="number">
	/// <item>
	/// Si <see cref="ForcontrolContext.varoperation()"/> es distinto de <c>null</c> lo ejecuta usando <see cref="IKysParserVisitor{Result}.VisitVaroperation(VaroperationContext)"/> usando el <see cref="IVisitor{T}"/> dado por <see cref="IVisitorProvider.GetVisitor{VisitorContext}"/> para el contexto <see cref="VaroperationContext"/>.
	/// </item>
	/// <item>
	/// El <see cref="ForcontrolContext.expression()"/> es ejecutado usando <see cref="IParseTreeVisitor{Result}.Visit(IParseTree)"/> usando el <see cref="IVisitor{T}"/> dado por <see cref="IVisitorProvider.GetVisitor{VisitorContext}"/> para el contexto <see cref="ExpressionContext"/>, si se evalua <c>true</c> entonces continua el siguiente paso, en caso contrario acaba el ciclo.
	/// </item>
	/// <item>
	/// Se ejecuta <see cref="ForcontrolContext.block()"/> con <see cref="VisitBlock(BlockContext)"/>.
	/// </item>
	/// <item>
	/// Si <see cref="ForcontrolContext.forexpression()"/> es distinto de <c>null</c> ejecuta la asignación o expresión con el <see cref="IVisitor{T}"/> dado por <see cref="IVisitorProvider"/> para los contextos <see cref="VaroperationContext"/> y <see cref="ExpressionContext"/> respectivamente.
	/// </item>
	/// <item>
	/// Vuelve al paso 2.
	/// </item>
	/// </list>
	/// </remarks>
	/// <inheritdoc/>
	public override object VisitForcontrol([NotNull] ForcontrolContext context)
	{
		Sesion.StartScope(ScopeType.CONTROL);
		var varop = context.varoperation();
		var exp = context.expression();
		var forexp = context.forexpression();
		var iexp = forexp?.expression();
		var ivarop = forexp?.varoperation();
		var block = context.block();

		//la operacion se ejecuta al principio, si existe
		if (varop != null) varoperationVisitor.VisitVaroperation(varop);

		// ejecutamos la expresion de condicion.
		while (Expistrue(exp))
		{
			// si se cumple ejecutamos el bloque
			VisitBlock(block);

			// si hay una expresion por ejecutar
			if (iexp != null) expressionVisitor.Visit(iexp);
			// si hay una operacion con variable
			else if (ivarop != null) varoperationVisitor.VisitVaroperation(ivarop);
		}

		Sesion.EndScope();
		return null;
	}

	/// <summary>
	/// Por defecto itera todas las sentencias de <see cref="BlockContext.sentence()"/> y las ejecuta usando <see cref="IKysParserVisitor{Result}.VisitSentence(SentenceContext)"/> usando el <see cref="IVisitor{T}"/> dado por <see cref="IVisitorProvider.GetVisitor{VisitorContext}"/> para el contexto <see cref="SentenceContext"/>.
	/// </summary>
	/// <inheritdoc/>
	public override object VisitBlock([NotNull] BlockContext context)
	{
		foreach (var item in context.sentence())
			sentenceVisitor.VisitSentence(item);
		return null;
	}
}
