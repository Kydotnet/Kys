using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Kys.Lang
{
	internal sealed class RuntimeContext : IContext
	{
		/// <summary>
		/// Guarda la lista de funciones definidas en este contexto.
		/// </summary>
		/// 
		private IDictionary<string, IFunction> Functions = new ConcurrentDictionary<string, IFunction>();

		public IScope RootScope { get; init; }

		public IEnumerable<IFunction> DefinedFunctions => Functions.Values;

		public bool CanExecute => true;

		bool IContext.IsStarted { get; set; }

		public bool AddFunction(IFunction Function) => Functions.TryAdd(Function?.Name, Function);

		public IFunction GetFunction(string Name) => 
			Functions.TryGetValue(Name, out IFunction function) ? function : null;

		public void OverrideFunction(IFunction Function) => Functions[Function?.Name] = Function;

		public bool RemoveFunction(string Name)
		{
			return ((IContext)this).IsStarted || !Functions.ContainsKey(Name) ? false : Functions.Remove(Name);
		}
	}
}