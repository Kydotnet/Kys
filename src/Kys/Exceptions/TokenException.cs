using Antlr4.Runtime;
using System;

namespace Kys.Exceptions
{
	[Serializable]
	public class TokenException : KysException
	{

		protected TokenException() : base() { }

		public TokenException(IToken token, string msg) : base(msg) =>
			Line = string.Format("line {0}:{1}", token.Line, token.Column);
	}
}