namespace Kys.Library;

/// <summary>
/// Indica que este metodo puede ser agregado como una función de Kys.
/// </summary>
[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public sealed class FunctionAttribute : Attribute
{
	/// <summary>
	/// Un <see cref="FunctionAttribute"/> vacio, utilize este valor en lugar de pasar <c>null</c> a los metodos que lo requieran.
	/// </summary>
	public static readonly FunctionAttribute None = new();

	/// <summary>
	/// Indica si el metodo va a recibir información del llamado, si es establecido en true entonces el primer parametro del metodo debe recibir un <see cref="Lang.IContext"/> y el segundo parametro debe recibir un <see cref="IScope"/>.
	/// </summary>
	public bool Passinfo { get; set; } = false;

	/// <summary>
	/// Nombre que se le pondra al metodo, por defecto el nombre es el mismo que el metodo de C#.
	/// </summary>
	public string Name { get; set; }
}
