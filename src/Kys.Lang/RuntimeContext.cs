using System.Collections.Concurrent;

namespace Kys.Lang;

public sealed class RuntimeContext : IContext
{
	/// <summary>
	/// Guarda la lista de funciones definidas en este contexto.
	/// </summary>
	/// 
	private IDictionary<string, IFunction> Functions = new ConcurrentDictionary<string, IFunction>();

	public RuntimeContext(IScope rootScope)
	{
		RootScope = rootScope;
	}

	public IScope RootScope { get; private set; }

	public IEnumerable<IFunction> DefinedFunctions => Functions.Values;

	public bool CanExecute => true;

	public bool AddFunction(IFunction Function) => Functions.TryAdd(Function?.Name, Function);

	public IFunction GetFunction(string Name) =>
		Functions.TryGetValue(Name, out IFunction function) ? function : null;

	public void OverrideFunction(IFunction Function) => Functions[Function?.Name] = Function;

	public bool RemoveFunction(string Name)
	{
		return Functions.ContainsKey(Name) && Functions.Remove(Name);
	}
}
