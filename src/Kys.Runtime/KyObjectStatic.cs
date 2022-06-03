global using static Kys.Runtime.KyObject;
using System.Reflection;

namespace Kys.Runtime;

public static class KyObject
{
	public static readonly IKyObject True = KyObject<bool>.FromValue(true);

	public static readonly IKyObject False = KyObject<bool>.FromValue(false);

	public static readonly IKyObject Null = new NullObject();

	private static readonly Type IKyType = typeof(KyObject<>);

	public static IKyObject FromUnknowValue(object? value)
	{
		if (value == null)
			return Null;
		if (value is IKyObject obj) return obj;
		var type = IKyType.MakeGenericType(value.GetType());
		var cons = type.GetConstructor(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly, new Type[] { value.GetType()});
		return (IKyObject)cons.Invoke(new object?[] { value });
	}

	public static IKyObject FromValue<T>(T value) where T : notnull
	{
		if (value == null)
			return Null;
		if (value is IKyObject obj) return obj;
		return KyObject<T>.FromValue(value);
	}

	public static void UnitaryOperation<T>(T unitaryOperation) where T : IIncrementOperators<T>
	{
		unitaryOperation++;	
	}
}