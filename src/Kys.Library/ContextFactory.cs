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
	public static class ContextFactory
	{
		private static Dictionary<ContextFactoryType, Func<IContext>>  Factory = new();

		private static IEnumerable<ContextFactoryType> types = Enum.GetValues<ContextFactoryType>().Reverse();

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="type"></param>
		public static void ChangeContext<T>(ContextFactoryType type) where T : IContext, new()
		{
			Factory[type] = () => new T()
			{
				RootScope = ScopeFactory.Create(ScopeFactoryType.ME)
			};
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public static IContext Create(ContextFactoryType type)
		{
			if(Factory.ContainsKey(type))
				return Factory[type]();
			foreach (var item in types)
			{
				if ((type & item) == item && Factory.ContainsKey(item))
					return Factory[item]();
			}
			return Factory[ContextFactoryType.ALL]();
		}

		[AutoLoad]
		static ContextFactory()
		{
			var MeType = typeof(IContext).Assembly.GetType("Kys.Lang.RuntimeContext");
			var MeSetScope = MeType.GetProperty("RootScope").SetMethod;
			Factory[ContextFactoryType.ALL] = () =>
			{
				var ret = Activator.CreateInstance(MeType) as IContext;
				MeSetScope.Invoke(ret, new object[] { ScopeFactory.Create(ScopeFactoryType.ME) });
				return ret;
			};
		}
	}
	/// <summary>
	/// 
	/// </summary>
	public enum ContextFactoryType
	{
		ALL		= 0b0,
		ME		= 0b1,
		KYS		= 0b11,
		PACKAGE = 0b101
	}
}
