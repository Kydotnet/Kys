using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Antlr4.Runtime.Tree;
namespace Kys.Interpreter.Visitors;

/// <summary>
/// Implementación por defecto de <see cref="IVisitor{T}"/> para ejecutar <see cref="ControlContext"/>.
/// </summary>
public class ControlVisitor : BaseVisitor<object>
{
#pragma warning disable CS8618
	IKysParserVisitor<dynamic> _expressionVisitor;
	IKysParserVisitor<object> _sentenceVisitor;
	IKysParserVisitor<object> _varoperationVisitor;
#pragma warning restore CS8618

	/// <inheritdoc/>
	public override void Configure(IServiceProvider serviceProvider)
	{
		base.Configure(serviceProvider);
		_expressionVisitor = VisitorProvider.GetVisitor<ExpressionContext>();
		_sentenceVisitor = VisitorProvider.GetVisitor<SentenceContext>();
		_varoperationVisitor = VisitorProvider.GetVisitor<VaroperationContext>();
	}

	/// <summary>
	/// Por defecto se evalua <see cref="IfcontrolContext.expression()"/> usando el <see cref="IVisitor{T}"/> para el contexto <see cref="ExpressionContext"/>, si es <c>true</c> se ejecuta <see cref="IfcontrolContext.block()"/> con <see cref="VisitBlock(BlockContext)"/>, de lo contrario en caso de que <see cref="IfcontrolContext.elsecontrol()"/> es distinto de <c>null</c> se eejcuta con <see cref="VisitElsecontrol(ElsecontrolContext)"/>.
	/// </summary>
	/// <inheritdoc/>
	public override object VisitIfcontrol([Antlr4.Runtime.Misc.NotNull] IfcontrolContext context)
	{
		Sesion.StartScope(ScopeType.Control);

		if (Expistrue(context.expression()))
			VisitBlock(context.block());
		else if (context.elsecontrol() != null)
			VisitElsecontrol(context.elsecontrol());
		Sesion.EndScope();
		return true;
	}

	/// <summary>
	/// Por defecto si se trata de un "else if" encadenado lo ejecuta con <see cref="VisitIfcontrol(IfcontrolContext)"/>, de lo contrario ejecuta el contenido del bloque "else" con <see cref="VisitBlock(BlockContext)"/>.
	/// </summary>
	/// <inheritdoc/>
	public override object VisitElsecontrol([Antlr4.Runtime.Misc.NotNull] ElsecontrolContext context)
	{
		if (context.ifcontrol() != null)
			VisitIfcontrol(context.ifcontrol());
		else
			VisitBlock(context.block());
		return true;
	}

	/// <summary>
	///  Por defecto se evalua <see cref="WhilecontrolContext.expression()"/> usando el <see cref="IVisitor{T}"/> para el contexto <see cref="ExpressionContext"/>, si es <c>true</c> se ejecuta <see cref="WhilecontrolContext.block()"/> con <see cref="VisitBlock(BlockContext)"/> y se vuelve a evaluar hasta que sea <c>false</c>.
	/// </summary>
	/// <inheritdoc/>
	public override object VisitWhilecontrol([Antlr4.Runtime.Misc.NotNull] WhilecontrolContext context)
	{
		Sesion.StartScope(ScopeType.Control);
		var block = context.block();
		var exp = context.expression();
		while (Expistrue(exp))
			VisitBlock(block);
		Sesion.EndScope();
		return true;
	}

	/// <summary>
	/// La estructura "timed while" es una estructura de control personalizada de kys y su funcionamiento es similar  a un while normal solo que permite proporcionar un tiempo en el cual la expresion es evaluada luego de terminar la ejecución anterior.
	/// </summary>
	/// <remarks>
	/// El bloque "timed while" s ele puede anexar un bloque "timeout" que especifica un valor maximo de tiempo a esperar el bucle, es decir, si se especifica un valor de <c>5000</c> entonces el bucle solo se ejecutara 5 segundos maximo, llegando solo a este limite si se ha evaluado como <c>true</c> siempre.
	/// El tiempo maximo del timeout es superado unicamente si la tarea interna de metodo tarda mas que el tiempo dado en timeout, por ejemplo, se da un tiempo de <c>100</c> pero ejecutar ese bloque toma 700ms, esto no interrumpira la ejecución del bloque si no que esperara a que finalize y ya no volvera a evaluar la expresion si no que ejecutara el bloque interno del timeout.
	/// </remarks>
	/// <inheritdoc/>
	public override object VisitTwhilecontrol([Antlr4.Runtime.Misc.NotNull] TwhilecontrolContext context)
	{
		Sesion.StartScope(ScopeType.Control);
		var info = context.twbucle();
		var block = info.block();
		var timed = info.timeoutcontrol();
		int wait = ValueVisitor.GetInt(info.NUMBER());
		var exp = info.expression();

		if (timed != null)
		{
			TwBucle(block, timed, wait, exp, InternalTWile);
		}
		else
		{
			while (Expistrue(exp))
			{
				VisitBlock(block);
				if (wait > 0)
					Task.Delay(wait).Wait();
			}
		}
		Sesion.EndScope();
		return true;
	}

	/// <summary>
	/// Permite ejecuta un bucle definido con un timeout dado.
	/// </summary>
	/// <param name="block">Bloque que se debe pasar al bucle,y este bloque es pasado directamente a <paramref name="whilefunc"/>.</param>
	/// <param name="timed">Contexto de la estructura timeout.</param>
	/// <param name="wait">Tiempo que debe esperar el bucle entre ejecución. Es pasado directamente a <paramref name="whilefunc"/></param>
	/// <param name="exp">Expresión que debe evaluar el bucle. Es pasado directamente a <paramref name="whilefunc"/></param>
	/// <param name="whilefunc">Metodo que deberia generar una tarea que representa la ejecución del bucle en segundo plano.</param>
	void TwBucle(BlockContext block, TimeoutcontrolContext timed, int wait, ExpressionContext exp, Func<ExpressionContext, CancellationTokenSource, int, BlockContext, Task> whilefunc)
	{
		var twait = ValueVisitor.GetInt(timed.NUMBER());
		var tblock = timed.block();
		var token = new CancellationTokenSource();

		// tarea de ejecución del bloque while
		var whiletask = whilefunc(exp, token, wait, block);
		// tarea de esperar el timeot
		var waittask = Task.Delay(twait, token.Token);
		// esperamos que se termine el while o a que se acabe el timeout
		var t = Task.WhenAny(whiletask, waittask).Result;
		// cancelamos el token para que se pare el timeout o para que ya el while no itere mas
		token.Cancel();

		if (t == waittask)
		{
			// si acaba primero el timeout entonces primero esperamos a que acabe la ejecución del bucle while actual(en caso de ser pesado puede tardar mas que el tiempo de timeout)
			whiletask.Wait();
			VisitBlock(tblock);

		}// si el que acabo primero fue el whiletask entonces es porque acabo antes que el timeout por loque no se debe hacer nada mas.

	}

	async Task InternalTWile(ExpressionContext exp, CancellationTokenSource token, int wait, BlockContext block)
	{
		while (Expistrue(exp) && !token.IsCancellationRequested)
		{
			VisitBlock(block);
			if (wait > 0)
				await Task.Delay(wait);
		}
	}

	/// <summary>
	/// La estructura wait es una estructura definida en Kys cuyo funcionamiento es el de esperar a que una expresión se cumpla.
	/// </summary>
	/// <remarks>
	/// Lo que hace el wait exactamente ese evaluar la expresión dada cada cierto tiempo, de forma indefinida hasta que esta evalue verdadero, en cuyo caso ejecutara el contenido del bloque.
	/// Para evitar que un bloque wait se quede esperando por siempre es posible usar un bloque timeout, que especifica un tiempo maximo para esperar a que  la condición se cumpla.
	/// </remarks>
	/// <inheritdoc/>
	public override object VisitWaitcontrol([Antlr4.Runtime.Misc.NotNull] WaitcontrolContext context)
	{
		Sesion.StartScope(ScopeType.Control);
		var info = context.twbucle();
		var block = info.block();
		var timed = info.timeoutcontrol();
		int wait = ValueVisitor.GetInt(info.NUMBER());
		var exp = info.expression();

		if (timed != null)
		{
			TwBucle(block, timed, wait, exp, InternalWait);
		}
		else
		{
			while (!Expistrue(exp))
			{
				if (wait > 0)
					Task.Delay(wait).Wait();
			}
			// solamente cuando la expresión evalua true se sale del bucle y se ejecuta el codigo
			VisitBlock(block);
		}

		Sesion.EndScope();
		return true;
	}

	async Task InternalWait(ExpressionContext exp, CancellationTokenSource token, int wait, BlockContext block)
	{
		while (!Expistrue(exp) && !token.IsCancellationRequested)
		{
			if (wait > 0)
				await Task.Delay(wait);
		}
		// solo ejecutamos el bloque wait si se llega aqui si averse cancelado la tarea, es decir, si llega antes que el timeout
		if (!token.IsCancellationRequested)
			// solamente cuando la expresión evalua true se sale del bucle y se ejecuta el codigo
			VisitBlock(block);
	}

	[SuppressMessage("Style", "IDE0046:Convertir a expresión condicional", Justification = "Convertir en retorno directo ocasiona que se use el callsite equivocado al compilar.")]
	bool Expistrue(ExpressionContext exp)
	{
		object val = _expressionVisitor.Visit(exp);
		// ReSharper disable once ConvertIfStatementToReturnStatement
		if (val == null || 
		    val.Equals(0) || 
		    val.Equals(false) || 
		    string.IsNullOrEmpty(val.ToString())) 
			return false;
			
		return true;
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
	public override object VisitForcontrol([Antlr4.Runtime.Misc.NotNull] ForcontrolContext context)
	{
		Sesion.StartScope(ScopeType.Control);
		var varop = context.varoperation();
		var exp = context.expression();
		var forexp = context.forexpression();
		var iexp = forexp?.expression();
		var ivarop = forexp?.varoperation();
		var block = context.block();

		//la operacion se ejecuta al principio, si existe
		if (varop != null) _varoperationVisitor.VisitVaroperation(varop);

		// ejecutamos la expresion de condicion.
		while (Expistrue(exp))
		{
			// si se cumple ejecutamos el bloque
			VisitBlock(block);

			// si hay una expresion por ejecutar
			if (iexp != null) _expressionVisitor.Visit(iexp);
			// si hay una operacion con variable
			else if (ivarop != null) _varoperationVisitor.VisitVaroperation(ivarop);
		}

		Sesion.EndScope();
		return true;
	}

	/// <summary>
	/// Por defecto itera todas las sentencias de <see cref="BlockContext.sentence()"/> y las ejecuta usando <see cref="IKysParserVisitor{Result}.VisitSentence(SentenceContext)"/> usando el <see cref="IVisitor{T}"/> dado por <see cref="IVisitorProvider.GetVisitor{VisitorContext}"/> para el contexto <see cref="SentenceContext"/>.
	/// </summary>
	/// <inheritdoc/>
	public override object VisitBlock([Antlr4.Runtime.Misc.NotNull] BlockContext context)
	{
		foreach (var item in context.sentence())
			_sentenceVisitor.VisitSentence(item);
		return true;
	}
}
