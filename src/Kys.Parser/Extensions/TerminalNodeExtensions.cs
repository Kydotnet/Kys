using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kys.Parser.Extensions
{
	public static class TerminalNodeExtensions
	{
		static Dictionary<ITerminalNode, string> keyValuePairs = new ();

		public static string GetCacheText(this ITerminalNode node)
		{
			if(keyValuePairs.ContainsKey(node))
				return keyValuePairs[node];
			keyValuePairs.Add(node, node.GetText());
			return keyValuePairs[node];
		}
	}
}
