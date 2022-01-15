using System;
using System.Collections.Generic;
using System.Text;

namespace Kys.Interpreter.Visitors
{
	public class BaseVisitor<T> : KysParserBaseVisitor<T>, IVisitor<T>
	{
		protected IInterpreterSesion Sesion;
		protected IVisitorProvider VisitorProvider;

		public virtual void Configure(IServiceProvider serviceProvider)
		{
			Sesion = (IInterpreterSesion)serviceProvider.GetService(typeof(IInterpreterSesion));
			VisitorProvider = (IVisitorProvider)serviceProvider.GetService(typeof(IVisitorProvider));
		}
	}
}
