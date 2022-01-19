namespace Kys.Interpreter;

/// <summary>
/// Implementación por defecto de <see cref="IInterpreterSesion"/>.
/// </summary>
public class KysInterpreterSesion : IInterpreterSesion
{
	/// <inheritdoc/>
	public IContext CurrentContext { get; set; }

	/// <inheritdoc/>
	public IScope CurrentScope { get; set; }

	/// <inheritdoc/>
	public IContext CallerContext { get; set; }

	readonly IDictionary<string, object> SesionVariables = new Dictionary<string, object>();

	/// <inheritdoc/>
	public object this[string name]
	{
		get => SesionVariables.TryGetValue(name, out object Variable) ? Variable : null;
		set => SesionVariables[name] = value;
	}

	readonly IScopeFactory ScopeFactory;

	/// <summary>
	/// Crea un nueva nueva sesión.
	/// </summary>
	/// <param name="scopeFactory">La factory que se usara para generar <see cref="IScope"/> en <see cref="IInterpreterSesion.StartScope(ScopeType)"/>.</param>
	public KysInterpreterSesion(IScopeFactory scopeFactory)
	{
		ScopeFactory = scopeFactory;
	}

	/// <inheritdoc/>
	public IScope EndScope()
	{
		var c = CurrentScope;

		CurrentScope = CurrentScope.ParentScope;

		return CurrentScope == null
			? throw new InvalidOperationException("A ocurrido un problema inesperado en la ejecución de la sesión del interprete.")
			: c;
	}

	/// <inheritdoc/>
	public IScope StartScope(ScopeType type)
	{
		var newScope = ScopeFactory.Create(type, CurrentScope);
		CurrentScope = newScope;
		return newScope;
	}
}
