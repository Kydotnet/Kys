namespace Kys.Interpreter;

public abstract partial class Interpreter
{
	public event EventHandler OnConfiguring;
	public event EventHandler OnInitialize;

	protected abstract bool IsOneRun { get; }

	protected int ExitCode { get; set; }

	public abstract void Configure();

	public virtual void Initialize()
	{
		var context = ContextFactory.Create(ContextFactoryType.ME);

		IContext.ChangeContext(context);

	}

	public int Start()
	{
		OnConfiguring?.Invoke(this, EventArgs.Empty);
		Configure();

		OnInitialize?.Invoke(this, EventArgs.Empty);
		Initialize();

		return ExitCode;
	}

}
