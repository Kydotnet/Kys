namespace Kys.Runtime;

internal class NullObject : IKyObject
{
	public NullObject()
	{
	}

	public IKyObject Aditive(bool pot, IKyObject b)
	{
		throw new NotImplementedException();
	}

	public bool Boolean()
	{
		return false;
	}

	public bool CanOperate(OperatorType type)
	{
		return (OperatorType.Boolean | OperatorType.Equality | OperatorType.UnitaryNegation).HasFlag(type);
	}

	public IKyObject Equality(bool pot, IKyObject b)
	{
		if (pot)
			return object.ReferenceEquals(null, b.ToCsharp()) ? True : False;
		return object.ReferenceEquals(null, b.ToCsharp()) ? False : True;
	}

	public IKyObject EqualityRelacional(bool pot, IKyObject b)
	{
		throw new NotImplementedException();
	}

	public IKyObject Logical(bool pot, IKyObject b)
	{
		throw new NotImplementedException();
	}

	public IKyObject Module(IKyObject b)
	{
		throw new NotImplementedException();
	}

	public IKyObject Multiplicative(bool pot, IKyObject b)
	{
		throw new NotImplementedException();
	}
		
	public IKyObject Potencial(bool pot, IKyObject b)
	{
		throw new NotImplementedException();
	}

	public IKyObject Relacional(bool pot, IKyObject b)
	{
		throw new NotImplementedException();
	}

	public object? ToCsharp()
	{
		return null;
	}

	public IKyObject UnaryNegation()
	{
		if (Boolean())
			return False;
		return True;
	}

	public IKyObject UnitaryOperation(bool increment)
	{
		throw new NotImplementedException();
	}
}