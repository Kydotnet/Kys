using System.Collections.Concurrent;

namespace Kys.Lang;

/// <summary>
/// Implementación por defecto de <see cref="IScope"/>.
/// </summary>
public class RuntimeScope : IScope
{
	public IScope ParentScope { get; set; }

	/// <summary>
	/// Contenedor interno de las variables
	/// </summary>>
	internal IDictionary<string, dynamic> Variables { get; set; } = new ConcurrentDictionary<string, dynamic>();

	public void Clear() => Variables.Clear();

	public void AsigVar(string ID, dynamic value, bool recursive = true)
	{
		if (recursive)
		{
			var scope = this.CheckRecursive(ID);
			if (scope != null)
			{
				scope.AsigVar(ID, value, false);
				return;
			}
		}
		if (!Variables.ContainsKey(ID))
			_ = Variables[ID];
		Variables[ID] = value;
	}

	public void SetVar(string ID, dynamic value, bool recursive = true)
	{
		if (recursive)
		{
			var scope = this.CheckRecursive(ID);
			if (scope != null)
			{
				scope.SetVar(ID, value, false);
				return;
			}
		}
		Variables[ID] = value;
	}

	public void DefVar(string ID, dynamic value, bool recursive = true)
	{
		if (recursive)
		{
			var scope = this.CheckRecursive(ID);
			if (scope != null)
				return;
		}
		else if (ConVar(ID)) return;
		Variables[ID] = value;
	}

	public void DecVar(string ID, dynamic value, bool recursive = true)
	{
		if (recursive)
		{
			var scope = this.CheckRecursive(ID);
			if (scope != null)
			{
				scope.DecVar(ID, value, false);
				return;
			}
		}
		Variables.Add(ID, value);
	}

	public dynamic GetVar(string ID, bool recursive = true)
	{
		if (Variables.TryGetValue(ID, out dynamic ret))
			return ret;
		if (recursive && ParentScope != null)
			return ParentScope.GetVar(ID);
		return Variables[ID];
	}

	public bool ConVar(string ID, bool recursive = false)
	{
		return recursive ? this.CheckRecursive(ID) != null : Variables.ContainsKey(ID);
	}
}