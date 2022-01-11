namespace Kys.Library;
public interface IContextFactory
{
	void ChangeContext<T>(ContextFactoryType type) where T : IContext;

	IContext Create(ContextFactoryType type);
}