using System;
using System.Reflection;
using Kys.Visitors;

namespace Kys.Core
{
	public class Function
	{
		public string Name { get; init; }

		public int ArgCount { get; init; }

		/// <summary>
		/// Representa un metodo de C#
		/// </summary>
		public Func Method { get; init; }

		public bool HasReturn { get; init; }

		/// <summary>
		/// Reperesenta un conjunto de sentencias que se ejcutaran cuando se llame a la función
		/// </summary>
		public KysParser.SentenceContext[] Sentences { get; init; }

		public dynamic Call(params dynamic[] args)
		{
			if (ArgCount > -1 && args.Length != ArgCount)
			{
				throw new ArgumentException($"La función {Name} requiere {ArgCount} parametros pero se pasaron {args.Length}");
			}
			if (Method != null)
			{
				return Method.Invoke(args);
			}
			else
			{
				// por ahora las funciones en kys no reciben parametros
				SentenceExecutor executor = new();
				// ejecutamos cada sentencia de la funcion.
				foreach (var item in Sentences)
					executor.Visit(item);
				return null;
			}
		}
	}

	public delegate dynamic Func(params dynamic[] args);
}