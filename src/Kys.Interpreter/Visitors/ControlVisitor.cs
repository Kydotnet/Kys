using Antlr4.Runtime.Misc;
using System.Threading;
using System.Threading.Tasks;

namespace Kys.Interpreter.Visitors
{
	public class ControlVisitor : BaseVisitor<object>
	{
		IKysParserVisitor<dynamic> expressionVisitor;
		IKysParserVisitor<object> sentenceVisitor;
		IKysParserVisitor<object> varoperationVisitor;

		public override void Configure(IServiceProvider serviceProvider)
		{
			base.Configure(serviceProvider);
			expressionVisitor = VisitorProvider.GetVisitor<ExpressionContext>();
			sentenceVisitor = VisitorProvider.GetVisitor<SentenceContext>();
			varoperationVisitor = VisitorProvider.GetVisitor<VaroperationContext>();
		}

		public override object VisitIfcontrol([NotNull] IfcontrolContext context)
		{
			Sesion.StartScope(ScopeFactoryType.CONTROL);
			dynamic exp = expressionVisitor.Visit(context.expression());
			if (exp)
				VisitBlock(context.block());
			else if (context.elsecontrol() != null)
				VisitElsecontrol(context.elsecontrol());
			Sesion.EndScope();
			return null;
		}

		public override object VisitElsecontrol([NotNull] ElsecontrolContext context)
		{
			if (context.ifcontrol() != null)
				VisitIfcontrol(context.ifcontrol());
			else
				VisitBlock(context.block());
			return null;
		}

		public override object VisitWhilecontrol([NotNull] WhilecontrolContext context)
		{
			Sesion.StartScope(ScopeFactoryType.CONTROL);
			var block = context.block();

			while (expressionVisitor.Visit(context.expression()))
				VisitBlock(block);
			Sesion.EndScope();
			return null;
		}

		public override object VisitTwhilecontrol([NotNull] TwhilecontrolContext context)
		{
			Sesion.StartScope(ScopeFactoryType.CONTROL);
			var info = context.twbucle();
			var block = info.block();
			var timed = info.timeoutcontrol();
			int wait = ValueVisitor.GetNumber(info.NUMBER());
			var exp = info.expression();

			if (timed != null)
			{
				var twait = ValueVisitor.GetNumber(timed.NUMBER());
				var tblock = timed.block();
				using var token = new CancellationTokenSource();

				var task = Task.Run(() =>
				{
					while (expressionVisitor.Visit(exp) && !token.IsCancellationRequested)
					{
						VisitBlock(block);
						Task.Delay(wait).Wait();
					}
					token.Cancel();
				});
				try
				{
					Task.Delay(twait, token.Token).Wait();
				}
				catch (Exception)
				{

				}
				finally
				{
					if (!token.IsCancellationRequested)
					{
						token.Cancel();
						// en caso de que el while se este ejecutando esperamos a que finalize, esto puede ocurrir cuando el bloque del while se pudo ejecutar y es algo muy pesado y aun no finaliza su ejecución
						task.Wait();
						VisitBlock(tblock);
					}
				}
			}
			else
			{
				while (expressionVisitor.Visit(exp))
				{
					VisitBlock(block);
					Task.Delay(wait).Wait();
				}
			}
			Sesion.EndScope();
			return null;
		}

		public override object VisitWaitcontrol([NotNull] WaitcontrolContext context)
		{
			Console.WriteLine("la estructura wait aun no ha sido implementada");
			return null;
		}

		public override object VisitForcontrol([NotNull] ForcontrolContext context)
		{
			Sesion.StartScope(ScopeFactoryType.CONTROL);
			var varop = context.varoperation();
			var exp = context.expression();
			var forexp = context.forexpression();
			var iexp = forexp?.expression();
			var ivarop = forexp?.varoperation();
			var block = context.block();

			//la operacion se ejecuta al principio, si existe
			if(varop != null) varoperationVisitor.VisitVaroperation(varop);

			// ejecutamos la expresion de condicion.
			while(expressionVisitor.Visit(exp))
			{
				// si se cumple ejecutamos el bloque
				VisitBlock(block);

				// si hay una expresion por ejecutar
				if (iexp != null) expressionVisitor.Visit(iexp);
				// si hay una operacion con variable
				else varoperationVisitor.VisitVaroperation(ivarop);
			}

			Sesion.EndScope();
			return null;
		}

		public override object VisitBlock([NotNull] BlockContext context)
		{
			foreach (var item in context.sentence())
				sentenceVisitor.VisitSentence(item);
			return null;
		}
	}
}