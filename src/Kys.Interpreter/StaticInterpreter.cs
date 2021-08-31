namespace Kys.Interpreter;

internal class StaticInterpreter : Interpreter
{
	protected override bool IsOneRun => true;

	public override void Configure()
	{
		
	}

	public override int Start()
	{
		return 0;
	}
}
