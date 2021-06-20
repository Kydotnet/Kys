namespace Kys.Lang
{
	public interface IFunction
	{
		string Name { get; }

		int ArgCount { get; }

		bool InfArgs { get; }

		IContext ParentContext { get; init; }

		dynamic Call(IContext CallerContext, IScope FunctionScope, dynamic[] args);
	}
}