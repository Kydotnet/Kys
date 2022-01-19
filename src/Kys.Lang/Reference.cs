namespace Kys.Lang;

/// <summary>
/// La clase Reference aun no esta en uso.
/// </summary>
public sealed class Reference
{
	/// <summary>
	/// Nombre de la variable a la que se hace referencia.
	/// </summary>
	public string Name { get; init; }

	/// <summary>
	/// Scope en el cual se encuentra la variable.
	/// </summary>
	public IScope Source { get; init; }

	/// <summary>
	/// El valor que tiene esta variable.
	/// </summary>
	public dynamic Value
	{
		get => Source.GetVar(Name, false);
		set => Source.SetVar(Name, value, false);
	}
}
