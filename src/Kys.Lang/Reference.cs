namespace Kys.Lang
{
	public sealed class Reference
	{
		/// <summary>
		/// Nombre de la variable a la que se hace referencia.
		/// </summary>
		public string Name { get; init; }

		/// <summary>
		/// Scope en el cuals e encuentra la variable.
		/// </summary>
		public IScope Source { get; init; }

		/// <summary>
		/// El valor que tiene esta variable.
		/// </summary>
		public dynamic Value
		{
			get => Source.GetVar(Name);
			set => Source.SetVar(Name, value);
		}
	}
}