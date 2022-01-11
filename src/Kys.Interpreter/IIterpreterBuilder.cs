namespace Kys.Interpreter;

public interface IIterpreterBuilder
{


	IIterpreterBuilder ConfigureContext(Action<IContext> configureDelegate);
 
	IInterpreter Build();
}