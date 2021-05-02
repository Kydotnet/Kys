using System;

namespace Kys.Exceptions
{
	[Serializable]
	public abstract class KysException : Exception
	{

		protected KysException() : this(string.Empty, "Ha ocurrido un error en la ejecución del programa.") { }

		protected KysException(string msg) : this(string.Empty, msg) { }

		public KysException(string interval, string msg) : base(msg)
		{
			Line = interval;
		}

		public string Line { get; protected set; }
	}

}