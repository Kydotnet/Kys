using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Kys.Lang
{
	/// <summary>
	/// Implementación por defecto de <see cref="IScope"/>.
	/// </summary>
	public class Scope : IScope
	{
		public IScope ParentScope { get; init; }

		/// <summary>
		/// Contenedor interno de las variables
		/// </summary>>
		internal IDictionary<string, dynamic> Variables { get; set; } = new ConcurrentDictionary<string, dynamic>();

		public void Clear() => Variables.Clear();
		public void AsigVar(string ID, dynamic value, bool recursive = true) => throw new NotImplementedException();
		public void SetVar(string ID, dynamic value, bool recursive = true) => throw new NotImplementedException();
		public void DefVar(string ID, dynamic value, bool recursive = true) => throw new NotImplementedException();
		public void DecVar(string ID, dynamic value, bool recursive = true) => throw new NotImplementedException();
		public dynamic GetVar(string ID, bool recursive = true) => throw new NotImplementedException();
	}
}