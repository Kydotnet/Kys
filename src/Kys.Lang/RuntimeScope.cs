using System.Collections.Concurrent;
namespace Kys.Lang;

/// <summary>
/// Implementación por defecto de <see cref="IScope"/>.
/// </summary>
public class RuntimeScope : IScope
{
	/// <inheritdoc/>
	public IScope? ParentScope { get; set; }

	/// <summary>
	/// Contenedor interno de las variables
	/// </summary>>
	internal IDictionary<string, dynamic?> Variables { get; set; } = new ConcurrentDictionary<string, dynamic?>();

	/// <inheritdoc/>
	public void Clear() => Variables.Clear();

	/// <inheritdoc/>
	public void AsigVar(string id, dynamic? value, bool recursive = true)
	{
		if (recursive)
		{
			var scope = this.CheckRecursive(id);
			if (scope != null)
			{
				scope.AsigVar(id, value, false);
				return;
			}
		}
		if (!Variables.ContainsKey(id))
			_ = Variables[id];
		Variables[id] = value;
	}

	/// <inheritdoc/>
	public void SetVar(string id, dynamic? value, bool recursive = true)
	{
		if (recursive)
		{
			var scope = this.CheckRecursive(id);
			if (scope != null)
			{
				scope.SetVar(id, value, false);
				return;
			}
		}
		Variables[id] = value;
	}

	/// <inheritdoc/>
	public void DefVar(string id, dynamic? value, bool recursive = true)
	{
		if (recursive)
		{
			var scope = this.CheckRecursive(id);
			if (scope != null)
				return;
		}
		else if (ConVar(id)) return;
		Variables[id] = value;
	}

	/// <inheritdoc/>
	public void DecVar(string id, dynamic? value, bool recursive = true)
	{
		if (recursive)
		{
			var scope = this.CheckRecursive(id);
			if (scope != null)
			{
				scope.DecVar(id, value, false);
				return;
			}
		}
		Variables.Add(id, value);
	}

	/// <inheritdoc/>
	public dynamic? GetVar(string id, bool recursive = true)
	{
		if (Variables.TryGetValue(id, out var ret))
			return ret;
		return recursive && ParentScope != null ? ParentScope.GetVar(id) : Variables[id];
	}

	/// <inheritdoc/>
	public bool ConVar(string id, bool recursive = false)
	{
		return recursive ? this.CheckRecursive(id) != null : Variables.ContainsKey(id);
	}
}