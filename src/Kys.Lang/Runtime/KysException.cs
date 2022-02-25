using Antlr4.Runtime;
namespace Kys.Lang.Runtime;

/// <summary>
/// Una excepción de kys.
/// </summary>
[Serializable]
public class KysException : Exception
{

	/// <summary>
	/// Crea una excepción con un mensaje por defecto.
	/// </summary>
	protected KysException() : this(string.Empty, "Ha ocurrido un error en la ejecución del programa.") { }

	/// <summary>
	/// Crea una nueva excepción ocasionada por un token y con un emnsaje especifico.
	/// </summary>
	/// <param name="exceptionToken"></param>
	/// <param name="msg"></param>
	public KysException(IToken exceptionToken, string msg) : base(msg)
	{
		Line = $"line {exceptionToken.Line}:{exceptionToken.Column}:";
	}

	protected KysException(string msg) : this(string.Empty, msg) { }

	public KysException(string interval, string msg) : base(msg)
	{
		Line = interval;
	}

	/// <summary>
	/// 
	/// </summary>
	public string Line { get; }
	
	/// <summary>
	/// 
	/// </summary>
	// ReSharper disable once ReturnTypeCanBeNotNullable
	public virtual string? Name => "Exception";
}