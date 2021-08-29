namespace Kys.Library;

[AutoLoad]
public static class ScopeFactory
{
	private static Dictionary<ScopeFactoryType, Func<IScope>> Factory = new();

	private static IEnumerable<ScopeFactoryType> types =
#if NET5_0_OR_GREATER
		Enum.GetValues<ScopeFactoryType>().Reverse();
#else
		Enum.GetValues(typeof(ScopeFactoryType)).Cast<ScopeFactoryType>().Reverse();
#endif

	public static void ChangeScope<T>(ScopeFactoryType type) where T : IScope, new() =>
		Factory[type] = () => new T();

	public static IScope Create(ScopeFactoryType type)
	{
		if (Factory.ContainsKey(type))
			return Factory[type]();
		foreach (var item in types)
		{
			if ((type & item) == item && Factory.ContainsKey(item))
				return Factory[item]();
		}
		return Factory[ScopeFactoryType.ALL]();
	}

	[AutoLoad]
	static ScopeFactory() => Factory.Add(ScopeFactoryType.ALL, () => new Scope());
}

public enum ScopeFactoryType
{
	ALL = 0b0,
	ME = 0b1,
	KYS = 0b101,
	PACKAGE = 0b1001,
	FUNCTION = 0b10,
	CONTROL = 0b110,

}
