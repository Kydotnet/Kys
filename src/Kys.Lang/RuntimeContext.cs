using System.Collections.Concurrent;

namespace Kys.Lang;

/// <summary>
/// La implementaci�n por defecto de <see cref="IContext"/>.
/// </summary>
public sealed class RuntimeContext : IContext
{
	/// <summary>
	/// Guarda la lista de funciones definidas en este contexto.
	/// </summary>
	/// 
	readonly IDictionary<string, IFunction> _functions = new ConcurrentDictionary<string, IFunction>();

	/// <summary>
	/// Crea un nuevo <see cref="RuntimeScope"/> usando como scope raiz <paramref name="rootScope"/>.
	/// </summary>
	/// <param name="rootScope"><see cref="IScope"/> que sera usado como raiz del contexto.</param>
	public RuntimeContext(IScope rootScope)
	{
		RootScope = rootScope;
	}

	/// <inheritdoc/>
	public IScope RootScope { get; }

	/// <inheritdoc/>
	public IEnumerable<IFunction> DefinedFunctions => _functions.Values;

	/// <inheritdoc/>
	public bool CanExecute => true;

	/// <inheritdoc/>
	public bool AddFunction(IFunction function) => _functions.TryAdd(function.Name, function);

	/// <inheritdoc/>
	public IFunction? GetFunction(string name) =>
		_functions.TryGetValue(name, out var function) ? function : null;

	/// <inheritdoc/>
	public void OverrideFunction(IFunction function) => _functions[function.Name] = function;

	/// <inheritdoc/>
	public bool RemoveFunction(string name) => _functions.ContainsKey(name) && _functions.Remove(name);
}
