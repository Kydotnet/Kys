using Antlr4.Runtime;
using Kys.Parser;

namespace Kys.Interpreter;

public interface IVisitorProvider
{
	void AddVisitor<VisitorContext, Implementation>() where VisitorContext : ParserRuleContext where Implementation : IKysParserVisitor<object>;

	void AddVisitor<VisitorContext, Implementation>(Implementation implementation) where VisitorContext : ParserRuleContext where Implementation : IKysParserVisitor<object>;

	IKysParserVisitor<dynamic> GetVisitor<VisitorContext>() where VisitorContext : ParserRuleContext;
}
 