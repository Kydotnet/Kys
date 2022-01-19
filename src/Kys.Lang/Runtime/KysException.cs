using Antlr4.Runtime;

namespace Kys.Lang.Runtime;

/// <summary>
/// Una excepción de kys.
/// </summary>
[Serializable]
public class KysException : Exception
{

	protected KysException() : this(string.Empty, "Ha ocurrido un error en la ejecución del programa.") { }

	public KysException(IToken exceptionToken, string msg) : base(msg)
	{
		Line = $"line {exceptionToken.Line}:{exceptionToken.Column}:";
	}

	protected KysException(string msg) : this(string.Empty, msg) { }

	public KysException(string interval, string msg) : base(msg)
	{
		Line = interval;
	}

	public string Line { get; protected set; }
}