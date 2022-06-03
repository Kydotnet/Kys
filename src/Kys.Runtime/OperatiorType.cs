namespace Kys.Runtime;

[Flags]
public enum OperatorType
{
	UnitaryOperation =		1,
	UnitaryNegation =		2,
	Potencial =				4,
	Multiplicative =		8,
	Module =				16,
	Aditive =				32,
	Relacional =			64,
	EqualityRelacional =	128,
	Equality =				256,
	Boolean =				512
}