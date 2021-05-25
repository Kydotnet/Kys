using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Kys.Lang
{
	public class Scope
	{
		public Scope Parent { get; init; }

		protected IDictionary<string, dynamic> Variables { get; set; } = new ConcurrentDictionary<string, dynamic>();

		public virtual void AsigVar(string ID, dynamic value)
		{
			if (Variables.ContainsKey(ID))
				Variables[ID] = value;
			throw new NullReferenceException($"Acceso a una variable {ID} no definida");
		}

		public virtual void SetVar(string ID, dynamic value) =>
			Variables[ID] = value;

		public virtual void DefVar(string ID, dynamic value) =>
			CollectionExtensions.TryAdd(Variables, ID, value);

		public virtual void DecVar(string ID, dynamic value) =>
			Variables.Add(ID, value);

		public virtual dynamic GetVar(string ID, bool recursive)
		{
			if (Variables.ContainsKey(ID))
				return Variables[ID];
			if (recursive && Parent != null)
				return Parent.GetVar(ID, true);
			else
				throw new ArgumentException($"Acceso a una variable {ID} no definida en el contexto actual", nameof(ID));
		}

		public dynamic GetVar(string ID) => GetVar(ID, true);

		public virtual dynamic this[string ID]
		{
			get => GetVar(ID, true);
			set => AsigVar(ID, value);
		}

	}
}