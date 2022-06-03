namespace Kys.Runtime;

internal class RuntimeHelper<T>
{
	public static IKyObject UnitaryOperation<T>(bool pot, T value) where T : IIncrementOperators<T>, IDecrementOperators<T>
	{
		if (pot) return FromValue(++value);
		return FromValue(++value);
	}

	public static IKyObject PotencialOperation<T>(bool pot, T value, IKyObject other) where T : IPotencialOperation<T>
	{
		if (pot) return FromValue(T.Pow(value, other));
		return FromValue(T.Root(value, other));
	}

	public static IKyObject PotencialOperationT<T>(bool pot, T value, T other) where T : INumber<T>
	{
		if (pot) return FromValue(Math.Pow(double.Create(value), double.Create(other)));
		return FromValue(Math.Pow(double.Create(other),1 / double.Create(value)));
	}

	public static IKyObject MultiplicativeOperation<T>(bool pot, T value, IKyObject other) where T : IMultiplyOperators<T, IKyObject, T>, IDivisionOperators<T, IKyObject, T>
	{
		if (pot) return FromValue(value * other);
		return FromValue(value / other);
	}

	public static IKyObject MultiplicativeOperationT<T>(bool pot, T value, T other) where T : IMultiplyOperators<T, T, T>, IDivisionOperators<T, T, T>
	{
		if (pot) return FromValue(value * other);
		return FromValue(value / other);
	}

	public static IKyObject ModuleOperation<T>(T value, IKyObject other) where T : IModulusOperators<T, IKyObject, T>
	{
		return FromValue(value % other);
	}

	public static IKyObject ModuleOperationT<T>(T value, T other) where T : IModulusOperators<T, T, T>
	{
		return FromValue(value % other);
	}

	public static IKyObject AditiveOperation<T>(bool pot, T value, IKyObject other) where T : IAdditionOperators<T, IKyObject, T>, ISubtractionOperators<T, IKyObject, T>
	{
		if (pot) return FromValue(value + other);
		return FromValue(value - other);
	}

	public static IKyObject AditiveOperationT<T>(bool pot, T value, T other) where T : IAdditionOperators<T, T, T>, ISubtractionOperators<T, T, T>
	{
		if (pot) return FromValue(value + other);
		return FromValue(value - other);
	}

	public static IKyObject RelacionalOperation<T>(bool pot, T value, IKyObject other) where T : IRelacionalOperation<T>
	{
		if(pot)
		{
			if (value < other) return True;
			return False;
		}
		if (value > other) return True;
		return False;
	}

	public static IKyObject RelacionalOperationT<T>(bool pot, T value, T other) where T : IComparisonOperators<T, T>
	{
		if (pot)
		{
			if (value < other) return True;
			return False;
		}
		if (value > other) return True;
		return False;
	}

	public static IKyObject EqualityRelacionalOperation<T>(bool pot, T value, IKyObject other) where T : IEqualityRelacionalOperation<T>
	{
		if (pot)
		{
			if (value <= other) return True;
			return False;
		}
		if (value >= other) return True;
		return False;
	}

	public static IKyObject EqualityRelacionalOperationT<T>(bool pot, T value, T other) where T : IComparisonOperators<T, T>
	{
		if (pot)
		{
			if (value <= other) return True;
			return False;
		}
		if (value >= other) return True;
		return False;
	}

	public static IKyObject EqualityOperation<T>(bool pot, T value, IKyObject other) where T : IEqualityOperation<T>
	{
		if (pot)
		{
			if (value == other) return True;
			return False;
		}
		if (value != other) return True;
		return False;
	}

	public static IKyObject EqualityOperationT<T>(bool pot, T value, T other) where T : IEqualityOperators<T, T>
	{
		if (pot)
		{
			if (value == other) return True;
			return False;
		}
		if (value != other) return True;
		return False;
	}
}
