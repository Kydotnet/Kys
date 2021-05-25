using System;
using System.Threading;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;
using Kys.Parser;

namespace Kys.Visitors
{
	public class ControlStructure : KysParserBaseVisitor<bool>
	{
		public static readonly ControlStructure Default = new();

		public override bool VisitIfcontrol([NotNull] KysParser.IfcontrolContext context)
		{
			dynamic exp = ExpressionResolver.Default.Visit(context.expression());
			if (exp)
				Visit(context.block());
			else if (context.elsecontrol() != null)
				Visit(context.elsecontrol());
			return true;
		}

		public override bool VisitElsecontrol([NotNull] KysParser.ElsecontrolContext context)
		{
			if (context.ifcontrol() != null)
				Visit(context.ifcontrol());
			else
				Visit(context.block());
			return true;
		}

		public override bool VisitWhilecontrol([NotNull] KysParser.WhilecontrolContext context)
		{
			var block = context.block();

			while (ExpressionResolver.Default.Visit(context.expression()))
				Visit(block);

			return true;
		}

		public override bool VisitTwhilecontrol([NotNull] KysParser.TwhilecontrolContext context)
		{
			var info = context.twbucle();
			var block = info.block();
			var timed = info.timeoutcontrol();
			int wait = ValueResolver.GetNumber(info.NUMBER());

			if (timed != null)
			{
				var twait = ValueResolver.GetNumber(timed.NUMBER());
				var tblock = timed.block();
				using var token = new CancellationTokenSource();

				var task = Task.Run(() =>
				{
					while (ExpressionResolver.Default.Visit(info.expression()) && !token.IsCancellationRequested)
					{
						Visit(block);
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
						Visit(tblock);
					}
				}
			}
			else
			{
				while (ExpressionResolver.Default.Visit(info.expression()))
				{
					Visit(block);
					Task.Delay(wait).Wait();
				}
			}
			return true;
		}

		public override bool VisitWaitcontrol([NotNull] KysParser.WaitcontrolContext context)
		{
			Console.WriteLine("la estructura wait aun no ha sido implementada");
			return true;
		}

		public override bool VisitForcontrol([NotNull] KysParser.ForcontrolContext context)
		{
			var step = ValueResolver.GetNumber(context.NUMBER());
			string varname;
			if (context.varoperation().declaration() != null)
			{
				varname = context.varoperation().declaration().asignation().ID().GetText();
			}
			else
			{
				varname = context.varoperation().asignation().ID().GetText();
			}
			SentenceExecutor.Default.VisitVaroperation(context.varoperation());

			while (ExpressionResolver.Default.Visit(context.expression()))
			{
				Visit(context.block());
				Program.Variables[varname] += step;
			}
			return true;
		}

		public override bool VisitBlock([NotNull] KysParser.BlockContext context)
		{
			foreach (var item in context.sentence())
				SentenceExecutor.Default.Visit(item);
			return true;
		}
	}
}