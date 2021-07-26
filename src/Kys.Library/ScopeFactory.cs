using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using KYLib.System;
using Kys.Lang;

namespace Kys.Library
{
	/// <summary>
	/// 
	/// </summary>
	[AutoLoad]
	public static class ScopeFactory
	{
		private static Dictionary<ScopeFactoryType, Func<IScope>>  Factory = new();

		private static IEnumerable<ScopeFactoryType> types = Enum.GetValues<ScopeFactoryType>().Reverse();

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="type"></param>
		public static void ChangeScope<T>(ScopeFactoryType type) where T : IScope, new() => 
			Factory[type] = () => new T();

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public static IScope Create(ScopeFactoryType type)
		{
			if (Factory.ContainsKey(type))
				return Factory[type]();
			foreach (var item in types)
			{
				if ((type & item) == item && Factory.ContainsKey(item))
					return Factory[item]();
			}
			return Factory[ScopeFactoryType.ALL]();
		}

		[AutoLoad]
		static ScopeFactory() => Factory.Add(ScopeFactoryType.ALL, () => new Scope());
	}

	/// <summary>
	/// 
	/// </summary>
	public enum ScopeFactoryType
	{
		ALL     = 0b0,
		ME      = 0b1,
		KYS     = 0b101,
		PACKAGE = 0b1001,
		FUNCTION= 0b10,
		CONTROL = 0b110,

	}
}
