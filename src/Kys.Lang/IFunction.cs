namespace Kys.Lang
{
	public interface IFunction
	{
		/// <summary>
		/// Nombre que toma la funci�n a la hora de llamarla.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Cantidad de parametros que recibe esta funci�n.
		/// En caso de que <see cref="InfArgs"/> sea <c>true</c> entonces este valor es el numero minimo de parametros que recibe la funci�n.
		/// </summary>
		int ArgCount { get; }

		/// <summary>
		/// Indica si la funci�n puede recibir una cantidad infinita de parametros.
		/// </summary>
		bool InfArgs { get; }

		/// <summary>
		/// Contexto en el cual se encuentra registrada esta funci�n.
		/// </summary>
		IContext ParentContext { get; init; }

		/// <summary>
		/// Ejecuta la funci�n por causa de un llamado.
		/// </summary>
		/// <param name="CallerContext">Contexto desde el cual se invoco a la funci�n.</param>
		/// <param name="FunctionScope">Scope que sera usado para almacenar variabes locales de la funci�n.</param>
		/// <param name="args">Argumentos pasados a la funci�n.</param>
		/// <returns>Valor devuelto por la funci�n.</returns>
		dynamic Call(IContext CallerContext, IScope FunctionScope, dynamic[] args);
	}
}