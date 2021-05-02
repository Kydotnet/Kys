using System;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Kys.Exceptions;

namespace Kys.Visitors
{
	internal class ValueResolver : KysParserBaseVisitor<dynamic>
	{
		public static ValueResolver Default = new();

		public override dynamic VisitValue([NotNull] KysParser.ValueContext context)
		{
			if (context.STRING() != null)
				return GetString(context.STRING());
			else if (context.NUMBER() != null)
				return GetNumber(context.NUMBER());
			else if (context.BOOL() != null)
				return GetBool(context.BOOL());
			else if (context.VAR() != null)
				return GetVar(context.VAR());

			return null;
		}

		public static bool GetBool(ITerminalNode terminalNode)
		{
			var raw = terminalNode.GetText();
			return raw.Equals("true");
		}

		public static dynamic GetVar(ITerminalNode terminalNode)
		{
			var raw = terminalNode.GetText();

			if (!Program.Variables.ContainsKey(raw))
				throw new UndefinedException(terminalNode.Symbol, raw);
			return Program.Variables[raw];
		}

		public static dynamic GetNumber(ITerminalNode terminalNode)
		{
			var raw = terminalNode.GetText();
			if (int.TryParse(raw, out int retint))
				return retint;
			if (double.TryParse(raw, out double retdou))
				return retdou;
			return 0;
		}

		public static string GetString(ITerminalNode terminalNode)
		{
			var raw = terminalNode.GetText();
			return raw.Trim('"');
		}
	}
}