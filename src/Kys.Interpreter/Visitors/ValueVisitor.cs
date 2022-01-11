using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;


namespace Kys.Interpreter.Visitors
{
	public class ValueVisitor : KysParserBaseVisitor<dynamic>
	{
		IInterpreterSesion Sesion;

		public ValueVisitor(IInterpreterSesion sesion)
		{
			Sesion = sesion;
		}

		public override dynamic VisitValue([NotNull] KysParser.ValueContext context)
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