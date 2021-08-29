namespace Kys.Library;

[AutoLoad]
public static class ScopeFactory
{
	private static Dictionary<ScopeFactoryType, Func<IScope, IScope>> Factory = new();

	private static IEnumerable<ScopeFactoryType> types =
#if NET5_0_OR_GREATER
		Enum.GetValues<ScopeFactoryType>().Reverse();
#else
		Enum.GetValues(typeof(ScopeFactoryType)).Cast<ScopeFactoryType>().Reverse();
#endif

	public static void ChangeScope<T>(ScopeFactoryType type) where T : IScope, new() =>
		Factory[type] = (p) => new T() { ParentScope = p };

	public static IScope Create(ScopeFactoryType type, IScope parent = null)
	{
		if (Factory.ContainsKey(type))
			return Factory[type](parent);
		foreach (var item in types)
		{
			if ((type & item) == item && Factory.ContainsKey(item))
				return Factory[item](parent);
		}
		return Factory[ScopeFactoryType.ALL](parent);
	}

	[AutoLoad]
	static ScopeFactory() => Factory.Add(ScopeFactoryType.ALL, (p) => new Scope() { ParentScope = p });
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
