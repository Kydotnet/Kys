namespace Kys.Interpreter;
public interface IInterpreter
{
	IContext ProgramContext { get; }

	IInterpreterSesion Sesion { get; }

	void Start(ProgramContext programContext);

	void Stop();

	void ConfigureContext();
}
 