using Antlr4.Runtime;
using Kys.Interpreter.Visitors;
using Kys.Parser;

namespace Kys.Interpreter;

public interface IVisitorProvider
{
	void AddVisitor<VisitorContext, Implementation>() where VisitorContext : ParserRuleContext where Implementation : IVisitor<object>;

	void AddVisitor<VisitorContext>(IVisitor<object> implementation) where VisitorContext : ParserRuleContext;

	void AddVisitor<VisitorContext>(Func<IServiceProvider, IVisitor<object>> implemtentation) where VisitorContext : ParserRuleContext;

	IVisitor<object> GetVisitor<VisitorContext>() where VisitorContext : ParserRuleContext;
}
 