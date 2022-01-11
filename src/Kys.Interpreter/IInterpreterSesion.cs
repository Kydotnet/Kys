namespace Kys.Interpreter;
public interface IInterpreterSesion
{
	IContext CurrentContext { get; set; }

	IScope CurrentScope { get; set; }

	IContext CallerContext { get; set; }

	IScope StartScope(ScopeFactoryType type);

	IScope EndScope();
}
