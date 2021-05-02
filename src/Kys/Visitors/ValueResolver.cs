using System;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Kys.Exceptions;

namespace Kys.Visitors
{
	internal class ValueResolver : KysParserBaseVisitor<dynamic>
	{
		public override dynamic VisitValue([NotNull] KysParser.ValueContext context)
		{
			if (context.STRING() != null)
				return GetString(context.STRING());
			else if (context.NUMBER() != null)
				return GetNumber(context.NUMBER());
			else if (context.VAR() != null)
				return GetVar(context.VAR());

			return null;
		}

		private static dynamic GetVar(ITerminalNode terminalNode)
		{
			var raw = terminalNode.GetText();

			if (!Program.Variables.ContainsKey(raw))
				throw new UndefinedException(terminalNode.Symbol, raw);
			return Program.Variables[raw];
		}

		private static dynamic GetNumber(ITerminalNode terminalNode)
		{
			var raw = terminalNode.GetText();
			if (int.TryParse(raw, out int retint))
				return retint;
			if (double.TryParse(raw, out double retdou))
				return retdou;
			return 0;
		}

		private static string GetString(ITerminalNode terminalNode)
		{
			var raw = terminalNode.GetText();
			return raw.Trim('"');
		}
	}
}