using System;
using System.Collections.Generic;
using System.Text;

namespace Kys.Interpreter
{
	public static class InterpreterBuilderExtensions
	{

		public static IInterpreterBuilder ConfigureDefaulContext(this IInterpreterBuilder builder)
		{

			builder.ConfigureContext(ConfigureContext);

			return builder;
		}

		private static void ConfigureContext(IContext obj)
		{
			obj.AddStandardFunctions();
		}
	}
}
