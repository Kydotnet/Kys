using System.IO;
using Antlr4.Runtime;
namespace Kys.Lang.Runtime;

/// <summary>
/// Error listener por defecto que produce una <see cref="KysException"/> cuando ocurre un error al parsear el programa.
/// </summary>
public class KysErrorListener : BaseErrorListener
{
	/// <summary>
	/// Produce una <see cref="KysException"/> que contiene el mensaje del error sintactico.
	/// </summary>
	/// <inheritdoc/>
	public override void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
	{
		throw new KysException(offendingSymbol, msg);
	}
}
