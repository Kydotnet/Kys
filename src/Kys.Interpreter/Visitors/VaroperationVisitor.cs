using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

namespace Kys.Interpreter.Visitors
{
	public class VaroperationVisitor : BaseVisitor<object>
	{
		IKysParserVisitor<dynamic> expressionVisitor;

		public override void Configure(IServiceProvider serviceProvider)
		{
			base.Configure(serviceProvider);

			expressionVisitor = VisitorProvider.GetVisitor<ExpressionContext>();
		}

		public override object VisitDeclaration([NotNull] DeclarationContext context)
		{
			VisitVaroperation(context.asignation(), Sesion.CurrentScope.DecVar);

			return null;
		}

		public override object VisitCreation([NotNull] CreationContext context)
		{
			VisitVaroperation(context.asignation(), Sesion.CurrentScope.SetVar);
			return null;
		}

		private void VisitVaroperation(AsignationContext context, Action<string, dynamic, bool> actionVar)
		{
			var name = context.ID().GetText();
			var valueExp = context.expression();
			object val = expressionVisitor.Visit(valueExp);

			actionVar(name, val, true);
		}

		public override object VisitDefinition([NotNull] DefinitionContext context)
		{
			VisitVaroperation(context.asignation(), Sesion.CurrentScope.DefVar);

			return null;
		}

		public override object VisitSimpleAssign([NotNull] SimpleAssignContext context)
		{
			VisitAsignation(context.ID(), context.expression(), context.Sequal());
			return null;
		}

		private void VisitAsignation(ITerminalNode name, ExpressionContext expressionContext, ITerminalNode simbol)
		{
			var id = name.GetText();
			var operation = simbol.GetText();
			dynamic value = expressionVisitor.Visit(expressionContext);
			Sesion["LastToken"] = simbol;
			switch (operation)
			{
				case "=": break;
				case "+=":
					value = Sesion.CurrentScope.GetVar(id) + value;
					break;
				case "-=":
					value = Sesion.CurrentScope.GetVar(id) - value;
					break;
				case "*=":
					value = Sesion.CurrentScope.GetVar(id) * value;
					break;
				case "/=":
					value = Sesion.CurrentScope.GetVar(id) / value;
					break;
				case "%=":
					value = Sesion.CurrentScope.GetVar(id) % value;
					break;
				case "^=":
					value = Math.Pow(Sesion.CurrentScope.GetVar(id), value);
					break;
				case "~=":
					value = Math.Pow(value, 1d / Sesion.CurrentScope.GetVar(id));
					break;

			}
			Sesion.CurrentScope.AsigVar(id, value);
		}

		public override object VisitPotencialAssign([NotNull] PotencialAssignContext context)
		{
			VisitAsignation(context.ID(), context.expression(), context.POTENCIALASSIGN());
			return null;
		}

		public override object VisitMultiplicativeAssign([NotNull] MultiplicativeAssignContext context)
		{
			VisitAsignation(context.ID(), context.expression(), context.MULTIPLICATIVEASSIGN());
			return null;
		}
		public override object VisitModuleAssign([NotNull] ModuleAssignContext context)
		{
			VisitAsignation(context.ID(), context.expression(), context.MODULEASSIGN());
			return null;
		}

		public override object VisitAditiveAssign([NotNull] AditiveAssignContext context)
		{
			VisitAsignation(context.ID(), context.expression(), context.ADITIVEASSIGN());
			return null;
		}
	}
}
