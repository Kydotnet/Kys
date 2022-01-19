using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Dfa;
using Antlr4.Runtime.Sharpen;
using System.IO;

namespace Kys.Lang.Runtime;

/// <summary>
/// Error listener por defecto que produce una <see cref="KysException"/> cuando ocurre un error al parsear el programa.
/// </summary>
public class KysErrorListener : BaseErrorListener
{
	/// <inheritdoc/>
	public override void ReportAmbiguity(Antlr4.Runtime.Parser recognizer, DFA dfa, int startIndex, int stopIndex, bool exact, BitSet ambigAlts, ATNConfigSet configs)
	{
		base.ReportAmbiguity(recognizer, dfa, startIndex, stopIndex, exact, ambigAlts, configs);
	}

	/// <inheritdoc/>
	public override void ReportAttemptingFullContext(Antlr4.Runtime.Parser recognizer, DFA dfa, int startIndex, int stopIndex, BitSet conflictingAlts, SimulatorState conflictState)
	{
		base.ReportAttemptingFullContext(recognizer, dfa, startIndex, stopIndex, conflictingAlts, conflictState);
	}

	/// <inheritdoc/>
	public override void ReportContextSensitivity(Antlr4.Runtime.Parser recognizer, DFA dfa, int startIndex, int stopIndex, int prediction, SimulatorState acceptState)
	{
		base.ReportContextSensitivity(recognizer, dfa, startIndex, stopIndex, prediction, acceptState);
	}

	/// <summary>
	/// Produce una <see cref="KysException"/> que contiene el mensaje del error sintactico.
	/// </summary>
	/// <inheritdoc/>
	public override void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
	{
		throw new KysException(offendingSymbol, msg);
	}
}
