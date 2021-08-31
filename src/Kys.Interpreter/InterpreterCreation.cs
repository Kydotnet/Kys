namespace Kys.Interpreter;

partial class Interpreter
{
	public static Interpreter Create(InterpreterType type)
	{
		return new StaticInterpreter();
	}
}
