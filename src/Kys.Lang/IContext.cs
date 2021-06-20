using System.Collections.Generic;
using System;

namespace Kys.Lang
{
	public interface IContext
	{
		public static IContext Me { get; internal set; } = new RuntimeContext();

		public static void ChangeContext(IContext context)
		{
			if (Me.IsStarted)
				throw new InvalidOperationException("No se puede cambiar el contexto una vez que ha iniciado la ejecución del actual.");
			if (context.CanExecute)
				Me = context;
		}

		public static void Start()
		{
			Me.IsStarted = true;
			Me.RootScope.Start();
		}

		public static bool Stop()
		{
			if (Me.RootScope.Stop())
			{
				Me.IsStarted = false;
				return true;
			}
			return false;
		}

		IScope RootScope { get; }

		IEnumerable<IFunction> DefinedFunctions { get; }

		bool AddFunction(IFunction Function);

		bool RemoveFunction(string Name);

		void OverrideFunction(IFunction Function);

		IFunction GetFunction(string Name);

		bool CanExecute { get; }

		bool IsStarted { get; internal set; }
}
}