using System;

namespace Kys.Runtime;

public interface IKyObject
{
	object? ToCsharp();

	bool CanOperate(OperatorType type);

	IKyObject UnitaryOperation(bool increment);
	IKyObject UnaryNegation();
	IKyObject Potencial(bool pot, IKyObject b);
	IKyObject Multiplicative(bool pot, IKyObject b);
	IKyObject Module(IKyObject b);
	IKyObject Aditive(bool pot, IKyObject b);
	IKyObject Equality(bool pot, IKyObject b);
	IKyObject EqualityRelacional(bool pot, IKyObject b);
	IKyObject Relacional(bool pot, IKyObject b);
	bool Boolean();
}