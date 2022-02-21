using System.Collections.Concurrent;
namespace Kys.Interpreter;

/// <summary>
/// Implementación por defecto de <see cref="IInterpreterSesion"/>.
/// </summary>
public class KysInterpreterSesion : IInterpreterSesion
{
	/// <inheritdoc/>
	public IInterpreter Interpreter { get; set; } = null!;

	/// <inheritdoc/>
	public IContext CurrentContext { get; set; } = null!;

	/// <inheritdoc/>
	public IScope CurrentScope { get; set; } = null!;

	/// <inheritdoc/>
	public IContext? CallerContext { get; set; }
	
	readonly IDictionary<string, object> _sesionVariables = new ConcurrentDictionary<string, object>();

	/// <inheritdoc/>
	public object? this[string name]
	{
		get => _sesionVariables.TryGetValue(name, out var variable) ? variable : null;
		set
		{
			if (value is null)
			{
				if (_sesionVariables.ContainsKey(name))
					_sesionVariables.Remove(name);
			}
			else
			{
				_sesionVariables[name] = value;
			}
		}
	}

	readonly IScopeFactory _scopeFactory;

	/// <summary>
	/// Crea un nueva nueva sesión.
	/// </summary>
	/// <param name="scopeFactory">La factory que se usara para generar <see cref="IScope"/> en <see cref="IInterpreterSesion.StartScope(ScopeType)"/>.</param>
	// ReSharper disable once NotNullMemberIsNotInitialized
	public KysInterpreterSesion(IScopeFactory scopeFactory)
	{
		_scopeFactory = scopeFactory;
	}

	/// <inheritdoc/>
	public void EndScope()
	{
		CurrentScope = CurrentScope.ParentScope ?? 
		throw new InvalidOperationException("A ocurrido un problema inesperado en la ejecución de la sesión del interprete.");
	}

	/// <inheritdoc/>
	public IScope StartScope(ScopeType type)
	{
		var newScope = _scopeFactory.Create(type, CurrentScope);
		CurrentScope = newScope;
		return newScope;
	}
}
