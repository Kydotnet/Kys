using Antlr4.Runtime.Tree;

namespace Kys.Parser;
partial class KysParser
{

	partial class ValueContext
	{
		ITerminalNode? _string;
		bool readString;
		ITerminalNode? _bool;
		bool readBool;
		ITerminalNode? _number;
		bool readNumber;
		ITerminalNode? _Id;
		bool readId;

		public ITerminalNode? String => GetCacheToken(ref _string, ref readString, KysParser.STRING);
		public ITerminalNode? Id => GetCacheToken(ref _Id, ref readId, KysParser.ID);
		public ITerminalNode? Bool => GetCacheToken(ref _bool, ref readBool, KysParser.BOOL);
		public ITerminalNode? Number => GetCacheToken(ref _number, ref readNumber, KysParser.NUMBER);

		private ITerminalNode? GetCacheToken(ref ITerminalNode? node, ref bool readed, int type)
		{
			if (readed) return node;
			readed = true;
			node = GetToken(type, 0);
			return node;
		}

	}
}