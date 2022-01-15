using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;


namespace Kys.Interpreter.Visitors
{
	public class ValueVisitor : BaseVisitor<dynamic>
	{
		IKysParserVisitor<dynamic> expressionVisitor;

		public override void Configure(IServiceProvider serviceProvider)
		{
			base.Configure(serviceProvider);
			expressionVisitor = VisitorProvider.GetVisitor<ExpressionContext>();
		}

		public override dynamic VisitFuncresult([NotNull] FuncresultContext context)
		{
			var funcname = context.ID().GetText();
			var nametoken = context.ID().Symbol;

			var func = Sesion.CurrentContext.GetFunction(funcname);

			var funcargs = context.arguments();
			var hasargs = funcargs != null;

			dynamic[] args;
			if (hasargs)
			{
				args = funcargs.expression().Select(v => expressionVisitor.Visit(v)).ToArray();
			}
			else
			{
				args = Array.Empty<dynamic>();
			}
			var scope = Sesion.StartScope(ScopeFactoryType.FUNCTION);

			//TODO: producir error cuando no existe la función;
			var dev = func.Call(Sesion.CurrentContext, scope, args);

			Sesion.EndScope();
			// si se devuelve algo que no sea nulo el InstructionVisitor terminara
			return dev;
		}

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

		public static bool GetBool(ITerminalNode terminalNode)
		{
			var raw = terminalNode.GetText();
			return raw.Equals("true");
		}

		public static dynamic GetVar(IInterpreterSesion sesion, ITerminalNode terminalNode)
		{
			var raw = terminalNode.GetText();

			return sesion.CurrentScope.GetVar(raw);
		}

		public static dynamic GetNumber(ITerminalNode terminalNode)
		{
			var raw = terminalNode.GetText();
			if (int.TryParse(raw, out int retint))
				return retint;
			return double.Parse(raw, System.Globalization.CultureInfo.InvariantCulture);
		}

		public static string GetString(ITerminalNode terminalNode)
		{
			var raw = terminalNode.GetText();
			return raw.Trim('"');
		}
	}
}