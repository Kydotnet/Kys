namespace Kys.Interpreter;

public interface IInterpreterBuilder
{


	IInterpreterBuilder ConfigureContext(Action<IContext> configureDelegate);
 
	IInterpreter Build();
}