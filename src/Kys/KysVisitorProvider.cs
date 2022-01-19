using Antlr4.Runtime;
using Kys.Interpreter;
using Kys.Interpreter.Visitors;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Kys;

public class KysVisitorProvider : IVisitorProvider
{
	readonly IDictionary<Type, IVisitor<object>> _visitors = new ConcurrentDictionary<Type, IVisitor<object>>();

	readonly IDictionary<Type, Type> _types = new ConcurrentDictionary<Type, Type>();

	readonly IVisitor<object> _visitor = new BaseVisitor<object>();

	readonly IServiceProvider serviceProvider;

	bool instanced = false;

	public KysVisitorProvider(IServiceProvider serviceProvider)
	{
		this.serviceProvider = serviceProvider;
	}

	public void AddVisitor<VisitorContext, Implementation>()
		where VisitorContext : ParserRuleContext
		where Implementation : IVisitor<object>
	{
		_types[typeof(VisitorContext)] = typeof(Implementation);
	}

	public void AddVisitor<VisitorContext>(IVisitor<object> implementation)
		where VisitorContext : ParserRuleContext
	{
		_visitors[typeof(VisitorContext)] = implementation;
	}

	public IVisitor<object> GetVisitor<VisitorContext>() where VisitorContext : ParserRuleContext
	{
		if (!instanced)
		{
			InstanceAll();
		}
		if (_visitors.ContainsKey(typeof(VisitorContext)))
			return _visitors[typeof(VisitorContext)];
		return _visitor;
	}

	internal void InstanceAll()
	{
		instanced = true;
		var registeredType = _types.Values.Distinct();
		var instances = registeredType.Select(
			t => (IVisitor<object>)ActivatorUtilities.CreateInstance(serviceProvider, t)
		).ToArray();

		var group = _types.GroupBy(T => T.Value).Zip(instances);

		foreach (var (First, Second) in group)
		{
			foreach (var type in First)
			{
				_visitors[type.Key] = Second;
			}
		}

		foreach (var visitor in instances)
			visitor.Configure(serviceProvider);
	}
}
