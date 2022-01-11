namespace Kys.Interpreter
{
	public class KysInterpreterSesion : IInterpreterSesion
	{
		public IContext CurrentContext { get; set; }

		public IScope CurrentScope { get; set; }

		public IContext CallerContext { get; set; }

		public int LastLine
		{
			get
			{
				return 0;
			}
			set
			{
				Console.WriteLine("Executing line {0}", value);
			}
		}

		IScopeFactory ScopeFactory;

		public KysInterpreterSesion(IScopeFactory scopeFactory)
		{
			ScopeFactory = scopeFactory;
		}

		public IScope EndScope()
		{
			var c = CurrentScope;

			CurrentScope = CurrentScope.ParentScope;

			if (CurrentScope == null)
				throw new InvalidOperationException("A ocurrido un problema inesperado en la ejecución de la sesión del interprete.");

			return c;
		}

		public IScope StartScope(ScopeFactoryType type)
		{
			var newScope = ScopeFactory.Create(type, CurrentScope);
			CurrentScope = newScope;
			return newScope;
		}
	}
}