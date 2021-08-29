namespace Kys.Library;

/// <summary>
/// Indica que este metodo puede ser agregado como una función de Kys.
/// </summary>
[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public sealed class FunctionAttribute : Attribute
{
	public static readonly FunctionAttribute None = new();

	/// <summary>
	/// Indica si el metodo va a recibir información del llamado, si es establecido en true entonces el primer parametro del metodo debe recibir un <see cref="Lang.IContext"/> y el segundo parametro debe recibir un <see cref="Lang.IScope"/>.
	/// </summary>
	public bool Passinfo { get; set; } = false;

	/// <summary>
	/// Nombre que se le pondra al metodo, por defecto el nombre es el mismo que el metodo de C#.
	/// </summary>
	public string Name { get; set; }
}
