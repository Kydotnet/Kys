namespace Kys.Library;

public enum ScopeFactoryType
{
	ALL = 0b0,
	ME = 0b1,
	KYS = 0b101,
	PACKAGE = 0b1001,
	FUNCTION = 0b10,
	CONTROL = 0b110,
}
