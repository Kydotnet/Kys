using Antlr4.Runtime;

namespace Kys.Exceptions
{
	[System.Serializable]
	public class UndefinedException : TokenException
	{
		public UndefinedException(IToken token, string varname) : base(
			token,
			$"acceso a una variable \"{varname}\" no definida")
		{ }

		protected UndefinedException() { }

	}
}