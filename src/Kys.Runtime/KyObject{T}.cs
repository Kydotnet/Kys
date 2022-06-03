using System;
using System.Reflection;

namespace Kys.Runtime;

public class KyObject<Cs> : CsObject where Cs : notnull
{
	Cs _value;

	static OperatorType operators;

	internal KyObject(Cs value)
	{
		_value = value;
	}

	internal static KyObject<Cs> FromValue(Cs b)
	{
		return new KyObject<Cs>(b);
	}

	public override bool CanOperate(OperatorType type) => operators.HasFlag(type);

	static Func<bool, Cs, IKyObject> UnitaryOperationHelper;

	public override IKyObject UnitaryOperation(bool increment) => UnitaryOperationHelper(increment, _value);

	public override object? ToCsharp() => _value;

	public override IKyObject UnaryNegation() => Boolean() ? False : True;

	static Func<bool, Cs, IKyObject, IKyObject> PotencialOperationHelper;

	public override IKyObject Potencial(bool pot, IKyObject b) => PotencialOperationHelper(pot, _value, b);

	static Func<bool, Cs, IKyObject, IKyObject> MultiplicativeOperationHelper;

	public override IKyObject Multiplicative(bool pot, IKyObject b) => MultiplicativeOperationHelper(pot, _value, b);

	static Func<Cs, IKyObject, IKyObject> ModuleOperationHelper;

	public override IKyObject Module(IKyObject b) => ModuleOperationHelper(_value, b);

	static Func<bool, Cs, IKyObject, IKyObject> AditiveOperationHelper;

	public override IKyObject Aditive(bool pot, IKyObject b) => AditiveOperationHelper(pot, _value, b);

	static Func<bool, Cs, IKyObject, IKyObject> RelacionalOperationHelper;

	public override IKyObject Relacional(bool pot, IKyObject b) => RelacionalOperationHelper(pot, _value, b);

	static Func<bool, Cs, IKyObject, IKyObject> EqualityRelacionalOperationHelper;

	public override IKyObject EqualityRelacional(bool pot, IKyObject b) => EqualityRelacionalOperationHelper(pot, _value, b);

	static Func<bool, Cs, IKyObject, IKyObject> EqualityOperationHelper;

	public override IKyObject Equality(bool pot, IKyObject b) => EqualityOperationHelper(pot, _value, b);

	public override bool Boolean()
	{
		return _value != null && !_value.Equals(0) && !_value.Equals(false);
	}

	static KyObject()
	{
		var t = typeof(Cs);
		var h = typeof(RuntimeHelper<Cs>);

		operators = OperatorType.Boolean | OperatorType.UnitaryNegation;

		// OperatorType.UnitaryOperation
		if (t.GetInterface("Kys.Runtime.IUnitaryOperation`1") is Type _ || (t.GetInterface("System.IIncrementOperators`1") is Type _ && t.GetInterface("System.IDecrementOperators`1") is Type _))
		{
			CreateAdaptor(OperatorType.UnitaryOperation, h, nameof(RuntimeHelper<Cs>.UnitaryOperation), out UnitaryOperationHelper);
		}

		Generate2(t,
			typeof(IPotencialOperation<>),
			OperatorType.Potencial,
			h,
			nameof(RuntimeHelper<Cs>.PotencialOperation),
			nameof(RuntimeHelper<Cs>.PotencialOperationT),
			ref PotencialOperationHelper!,
			typeof(INumber<>));

		Generate3(t,
			typeof(IMultiplicativeOperation<>),
			OperatorType.Multiplicative,
			h,
			nameof(RuntimeHelper<Cs>.MultiplicativeOperation),
			nameof(RuntimeHelper<Cs>.MultiplicativeOperationT),
			ref MultiplicativeOperationHelper!,
			typeof(IMultiplyOperators<,,>),
			typeof(IDivisionOperators<,,>));

		// OperatorType.Module

		if (t.GetInterface("System.IModulusOperators`3") is Type i3)
		{
			if (i3.GenericTypeArguments[1] == typeof(IKyObject))
			{
				CreateAdaptor(OperatorType.Module, h, nameof(RuntimeHelper<Cs>.ModuleOperation), out ModuleOperationHelper);
			}
			else
			{
				CreateBiAdaptor(OperatorType.Module, h, nameof(RuntimeHelper<Cs>.ModuleOperationT), out ModuleOperationHelper);

			}
		}

		Generate3(t,
			typeof(IAditiveOperation<>),
			OperatorType.Aditive,
			h,
			nameof(RuntimeHelper<Cs>.AditiveOperation),
			nameof(RuntimeHelper<Cs>.AditiveOperationT),
			ref AditiveOperationHelper!,
			typeof(IAdditionOperators<,,>),
			typeof(ISubtractionOperators<,,>));

		Generate3(t,
			typeof(IRelacionalOperation<>),
			OperatorType.Relacional,
			h,
			nameof(RuntimeHelper<Cs>.RelacionalOperation),
			nameof(RuntimeHelper<Cs>.RelacionalOperationT),
			ref RelacionalOperationHelper!,
			typeof(IComparisonOperators<,>),
			typeof(IComparisonOperators<,>));

		Generate3(t,
			typeof(IEqualityRelacionalOperation<>),
			OperatorType.EqualityRelacional,
			h,
			nameof(RuntimeHelper<Cs>.EqualityRelacionalOperation),
			nameof(RuntimeHelper<Cs>.EqualityRelacionalOperationT),
			ref EqualityRelacionalOperationHelper!,
			typeof(IComparisonOperators<,>),
			typeof(IComparisonOperators<,>));

		Generate3(t,
			typeof(IEqualityOperation<>),
			OperatorType.Equality,
			h,
			nameof(RuntimeHelper<Cs>.EqualityOperation),
			nameof(RuntimeHelper<Cs>.EqualityOperationT),
			ref EqualityOperationHelper!,
			typeof(IEqualityOperators<,>),
			typeof(IEqualityOperators<,>));


	}

	private static void Generate2(Type t, Type type, OperatorType aditive, Type h, string v1, string v2, ref Func<bool, Cs, IKyObject, IKyObject> aditiveOperationHelper, Type type2)
	{
		if (t.GetInterface(type.FullName) is Type _)
		{
			CreateAdaptor(aditive, h, v1, out aditiveOperationHelper);
		}
		else if (t.GetInterface(type2.FullName) is Type i1)
		{
			CreateBiAdaptor(aditive, h, v2, out aditiveOperationHelper);
		}
	}
	private static void Generate3(Type t, Type type, OperatorType aditive, Type h, string v1, string v2, ref Func<bool, Cs, IKyObject, IKyObject> aditiveOperationHelper, Type type2, Type type3)
	{
		if (t.GetInterface(type.FullName) is Type _)
		{
			CreateAdaptor(aditive, h, v1, out aditiveOperationHelper);
		}
		else if (t.GetInterface(type2.FullName) is Type i1 && t.GetInterface(type3.FullName) is Type i2)
		{
			if (i1.GenericTypeArguments[1] == i2.GenericTypeArguments[1] && i1.GenericTypeArguments[1] == typeof(IKyObject))
			{
				CreateAdaptor(aditive, h, v1, out aditiveOperationHelper);
			}
			else
			{
				CreateBiAdaptor(aditive, h, v2, out aditiveOperationHelper);
			}
		}
	}

	private static void CreateBiAdaptor(OperatorType multiplicative, Type h, string v, out Func<bool, Cs, IKyObject, IKyObject> del)
	{
		operators |= multiplicative;
		var m = h.GetMethod(v)?
			.MakeGenericMethod(typeof(Cs));
		var met = m.CreateDelegate <Func<bool, Cs, Cs, IKyObject>>(null);
		del = (a, b, c) => {
			if(c.ToCsharp() is Cs cs)
				return met(a, b, cs);
			try
			{
				return (IKyObject?)m!.Invoke(null, new object?[] { a, b, c.ToCsharp() }) ?? Null;
			}
			catch (ArgumentException e)
			{
				throw new NotImplementedException(e.Message, e);
			}
		};
	}

	private static void CreateBiAdaptor(OperatorType multiplicative, Type h, string v, out Func<Cs, IKyObject, IKyObject> del)
	{
		operators |= multiplicative;
		var m = h.GetMethod(v)?
			.MakeGenericMethod(typeof(Cs));
		del = (b, c) => {
			try
			{
				return (IKyObject?)m!.Invoke(null, new object?[] { b, c.ToCsharp() }) ?? Null;
			}
			catch (ArgumentException e)
			{
				throw new NotImplementedException(e.Message, e);
			}
		};
	}

	private static void CreateAdaptor<T>(OperatorType operation, Type h, string v, out T del) where T : Delegate
	{
		operators |= operation;
		var m = h.GetMethod(v)?.MakeGenericMethod(typeof(Cs));
		del =  m!.CreateDelegate<T>(null);
	}

	public static implicit operator Cs(KyObject<Cs> val) => val._value;

}