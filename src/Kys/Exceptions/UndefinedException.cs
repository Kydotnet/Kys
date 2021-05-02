using Antlr4.Runtime;

namespace Kys.Exceptions
{
	[System.Serializable]
	public class UndefinedException : TokenException
	{
		public UndefinedException(IToken token, string varname) : this(
			token,
			varname,
			"variable")
		{ }

		public UndefinedException(IToken token, string varname, string v) : base(
			token,
			$"acceso a una {v} \"{varname}\" no definida")
		{ }

		protected UndefinedException() { }

	}

	[System.Serializable]
	public class UndefinedFunctionException : UndefinedException
	{
		public UndefinedFunctionException(IToken token, string varname) : base(token, varname, "función")
		{
		}

		protected UndefinedFunctionException() { }
	}
}