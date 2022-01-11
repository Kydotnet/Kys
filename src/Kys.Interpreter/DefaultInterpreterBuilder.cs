namespace Kys.Interpreter
{
	internal class DefaultInterpreterBuilder : IInterpreterBuilder
	{
		private IContext context;
		IInterpreterSesion sesion;
		IVisitorProvider visitorProvider;

		public DefaultInterpreterBuilder(IContextFactory contextFactory, IInterpreterSesion interpreterSesion, IVisitorProvider visitorProvider)
		{
			context = contextFactory.Create(ContextFactoryType.ME);
			sesion = interpreterSesion;
			this.visitorProvider = visitorProvider;
		}

		public IInterpreter Build()
		{
			return new KysInterpreter()
			{
				ProgramContext = context,
				Sesion = sesion,
				KysParserVisitor = visitorProvider.GetVisitor<ProgramContext>()
			};
		}

		public IInterpreterBuilder ConfigureContext(Action<IContext> configureDelegate)
		{
			configureDelegate(context);
			return this;
		}
	}
}