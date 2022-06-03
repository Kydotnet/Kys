namespace Kys.Runtime;

public abstract class CsObject : IKyObject
{
	public abstract bool CanOperate(OperatorType type);

	public abstract object? ToCsharp();
	public abstract IKyObject UnitaryOperation(bool increment);
	public abstract IKyObject UnaryNegation();
	public abstract IKyObject Potencial(bool pot, IKyObject b);
	public abstract IKyObject Multiplicative(bool pot, IKyObject b);
	public abstract IKyObject Module(IKyObject b);
	public abstract IKyObject Aditive(bool pot, IKyObject b);
	public abstract IKyObject Equality(bool pot, IKyObject b);
	public abstract IKyObject EqualityRelacional(bool pot, IKyObject b);
	public abstract IKyObject Relacional(bool pot, IKyObject b);
	public abstract bool Boolean();
}