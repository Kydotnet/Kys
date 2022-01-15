using System;
using System.Collections.Generic;
using System.Text;

namespace Kys.Interpreter.Visitors
{
	public interface IVisitor<T> : IKysParserVisitor<T>
	{
		void Configure(IServiceProvider serviceProvider);
	}
}
