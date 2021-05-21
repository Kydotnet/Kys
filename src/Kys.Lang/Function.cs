using System;
namespace Kys.Lang
{
	public class Function
	{
		public string Name { get; init; }

		public int ArgCount { get; init; }

		/// <summary>
		/// Representa un metodo de C#
		/// </summary>
		public Func Method { get; init; }

		public virtual dynamic Call(params dynamic[] args)
		{
			if (ArgCount > -1 && args.Length != ArgCount)
			{
				throw new ArgumentException($"La función {Name} requiere {ArgCount} parametros pero se pasaron {args.Length}");
			}
			if (Method != null)
			{
				return Method.Invoke(args);
			}
			return null;
		}
	}
}