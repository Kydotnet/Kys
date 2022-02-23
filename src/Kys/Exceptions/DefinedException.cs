using System;
using Antlr4.Runtime;
namespace Kys.Exceptions
{
	[Serializable]
	public class DefinedException : TokenException
	{
		public DefinedException(IToken token, string varname) : base(
			token,
			$"la variable \"{varname}\" ya ha sido definida")
		{ }

		protected DefinedException() { }

	}
}