using System;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Kys.Exceptions;

namespace Kys.Visitors
{
	internal class ValueResolver : KysParserBaseVisitor<dynamic>
	{
		public static readonly ValueResolver Default = new();

		public override dynamic VisitValue([NotNull] KysParser.ValueContext context)
		{
			if (context.STRING() != null)
				return GetString(context.STRING());
			else if (context.NUMBER() != null)
				return GetNumber(context.NUMBER());
			else if (context.BOOL() != null)
				return GetBool(context.BOOL());
			else if (context.ID() != null)
				return GetVar(context.ID());

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
			return double.Parse(raw, System.Globalization.CultureInfo.InvariantCulture);
		}

		public static string GetString(ITerminalNode terminalNode)
		{
			var raw = terminalNode.GetText();
			return raw.Trim('"');
		}
	}
}