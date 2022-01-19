using std = Kys.Library.StandardFunctions;
namespace Kys.Interpreter;

/// <summary>
/// Extenciones utiles paracrear implementaciones por defecto de Kys.
/// </summary>
public static class InterpreterBuilderExtensions
{
	/// <summary>
	/// Agrega la configuación por defecto a un <see cref="IContext"/>.
	/// </summary>
	/// <param name="builder">El <see cref="IInterpreter"/> al cual se agregaran funciones.</param>
	/// <returns>Devuelve el mismo objeto <paramref name="builder"/> para encadenamiento.</returns>
	public static IInterpreter ConfigureDefaultContext(this IInterpreter builder)
	{

		ConfigureContext(builder.ProgramContext);
		
		return builder;
	}

	/// <summary>
	/// Agrega las funciones estandar a un contexto.
	/// </summary>
	/// <param name="obj">Contexto de destino.</param>
	private static void ConfigureContext(IContext obj)
	{
		obj.AddStandardFunctions();
	}
}
