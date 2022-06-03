using KYLib.Abstractions;
using Kys.Runtime;

namespace Kys.Lang;

/// <summary>
/// Una función que puede ser agregada a un contexto de ejecución (<see cref="IContext"/>) para ser llamada desde Kys.
/// </summary>
public interface IFunction : INameable
{
	/// <summary>
	/// Cantidad de parametros que recibe esta función.
	/// En caso de que <see cref="InfArgs"/> sea <c>true</c> entonces este valor es el numero minimo de parametros que recibe la función.
	/// </summary>
	int ArgCount { get; }

	/// <summary>
	/// Indica si la función puede recibir una cantidad infinita de parametros.
	/// </summary>
	bool InfArgs { get; }

	/// <summary>
	/// Contexto en el cual se encuentra registrada esta función.
	/// </summary>
	IContext ParentContext { get; init; }

	/// <summary>
	/// Ejecuta la función por causa de un llamado.
	/// </summary>
	/// <param name="callerContext">Contexto desde el cual se invoco a la función.</param>
	/// <param name="functionScope">Scope que sera usado para almacenar variabes locales de la función.</param>
	/// <param name="args">Argumentos pasados a la función.</param>
	/// <returns>Valor devuelto por la función.</returns>
	IKyObject Call(IContext callerContext, IScope functionScope, params IKyObject[] args);
}
