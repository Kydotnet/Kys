using Antlr4.Runtime.Misc;

namespace Kys.Interpreter.Visitors
{
	public class SentenceVisitor : BaseVisitor<object>
	{
		IKysParserVisitor<dynamic> controlVisitor;
		IKysParserVisitor<dynamic> varOperationVisitor;
		IKysParserVisitor<dynamic> funcresultVisitor;

		public override void Configure(IServiceProvider serviceProvider)
		{
			base.Configure(serviceProvider);
			controlVisitor = VisitorProvider.GetVisitor<ControlContext>();
			varOperationVisitor = VisitorProvider.GetVisitor<VaroperationContext>();
			funcresultVisitor = VisitorProvider.GetVisitor<FuncresultContext>();
		}

		public override object VisitSentence([NotNull] SentenceContext context)
		{
			Sesion["LastColumn"] = context.Start.Column;
			Sesion["LastSentence"] = context;
			return base.VisitSentence(context);
		}

		public override object VisitControl([NotNull] ControlContext context) =>
			controlVisitor.VisitControl(context);

		public override object VisitVaroperation([NotNull] VaroperationContext context) =>
			varOperationVisitor.VisitVaroperation(context);

		public override object VisitFunccall([NotNull] FunccallContext context)
		{
			funcresultVisitor.VisitFuncresult(context.funcresult());

			// si se devuelve algo que no sea nulo el InstructionVisitor terminara
			return null;
		}
	}
}