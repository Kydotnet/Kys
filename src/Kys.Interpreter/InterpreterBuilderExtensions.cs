using System;
using System.Collections.Generic;
using System.Text;

namespace Kys.Interpreter
{
	public static class InterpreterBuilderExtensions
	{

		public static IInterpreter ConfigureDefaultContext(this IInterpreter builder)
		{

			ConfigureContext(builder.ProgramContext);

			return builder;
		}

		private static void ConfigureContext(IContext obj)
		{
			obj.AddStandardFunctions();
		}
	}
}
