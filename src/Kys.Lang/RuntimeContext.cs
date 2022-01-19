using System.Collections.Concurrent;

namespace Kys.Lang;

/// <summary>
/// La implementación por defecto de <see cref="IContext"/>.
/// </summary>
public sealed class RuntimeContext : IContext
{
	/// <summary>
	/// Guarda la lista de funciones definidas en este contexto.
	/// </summary>
	/// 
	readonly IDictionary<string, IFunction> Functions = new ConcurrentDictionary<string, IFunction>();

	/// <summary>
	/// Crea un nuevo <see cref="RuntimeScope"/> usando como scope raiz <paramref name="rootScope"/>.
	/// </summary>
	/// <param name="rootScope"><see cref="IScope"/> que sera usado como raiz del contexto.</param>
	public RuntimeContext(IScope rootScope)
	{
		RootScope = rootScope;
	}

	/// <inheritdoc/>
	public IScope RootScope { get; private set; }

	/// <inheritdoc/>
	public IEnumerable<IFunction> DefinedFunctions => Functions.Values;

	/// <inheritdoc/>
	public bool CanExecute => true;

	/// <inheritdoc/>
	public bool AddFunction(IFunction Function) => Functions.TryAdd(Function?.Name, Function);

	/// <inheritdoc/>
	public IFunction GetFunction(string Name) =>
		Functions.TryGetValue(Name, out IFunction function) ? function : null;

	/// <inheritdoc/>
	public void OverrideFunction(IFunction Function) => Functions[Function?.Name] = Function;

	/// <inheritdoc/>
	public bool RemoveFunction(string Name)
	{
		return Functions.ContainsKey(Name) && Functions.Remove(Name);
	}
}
