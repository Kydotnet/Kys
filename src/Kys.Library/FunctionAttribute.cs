namespace Kys.Library
{
	[System.AttributeUsage(System.AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
	sealed class FunctionAttribute : System.Attribute
	{
		readonly string name;
		readonly int argcount;
		readonly bool hasreturn;

		public FunctionAttribute(string name, int argcount, bool hasreturn)
		{
			this.name = name;
			this.argcount = argcount;
			this.hasreturn = hasreturn;
		}

		public string Name => name;

		public bool HasReturn => hasreturn;

		public int Argcount => argcount;
	}
}