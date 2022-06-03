namespace Kys.Runtime;

public interface IUnitaryOperation<T> : IIncrementOperators<T>, IDecrementOperators<T> where T : IUnitaryOperation<T>
{
}

public interface IPotencialOperation<T> where T : IPotencialOperation<T>
{
	public static abstract IKyObject Pow(T left, IKyObject right);

	public static abstract IKyObject Root(T left, IKyObject right);
}

public interface IMultiplicativeOperation<T> : IMultiplyOperators<T, IKyObject, T>, IDivisionOperators<T, IKyObject, T> where T : IMultiplicativeOperation<T>
{
}

public interface IAditiveOperation<T> : IAdditionOperators<T, IKyObject, T>, ISubtractionOperators<T, IKyObject, T> where T : IAditiveOperation<T>
{
}

public interface IRelacionalOperation<T> where T : IRelacionalOperation<T>
{
	public static abstract bool operator <(T left, IKyObject right);
	public static abstract bool operator >(T left, IKyObject right);
}

public interface IEqualityRelacionalOperation<T> where T : IEqualityRelacionalOperation<T>
{
	public static abstract bool operator <=(T left, IKyObject right);
	public static abstract bool operator >=(T left, IKyObject right);
}

public interface IEqualityOperation<T> : IEqualityOperators<T,IKyObject> where T : IEqualityOperation<T>
{
}