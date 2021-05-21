namespace Kys.Library
{
	[System.AttributeUsage(System.AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
	sealed class FunctionAttribute : System.Attribute
	{
		readonly string name;
		readonly int argcount;

		public FunctionAttribute(string name, int argcount)
		{
			this.name = name;
			this.argcount = argcount;
		}

		public string Name => name;

		public int Argcount => argcount;
	}
}