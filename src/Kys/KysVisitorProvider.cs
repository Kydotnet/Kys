using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime;
using Kys.Interpreter;
using Kys.Interpreter.Visitors;
using Microsoft.Extensions.DependencyInjection;
namespace Kys;

public class KysVisitorProvider : IVisitorProvider
{
	readonly IDictionary<Type, IVisitor<object>> _visitors = new ConcurrentDictionary<Type, IVisitor<object>>();

	readonly IDictionary<Type, Type> _types = new ConcurrentDictionary<Type, Type>();

	readonly IVisitor<object> _visitor = new BaseVisitor<object>();

	readonly IServiceProvider _serviceProvider;

	bool _instanced;

	public KysVisitorProvider(IServiceProvider serviceProvider)
	{
		_serviceProvider = serviceProvider;
	}

	public void AddVisitor<TVisitorContext, TImplementation>()
		where TVisitorContext : ParserRuleContext
		where TImplementation : IVisitor<object>
	{
		_types[typeof(TVisitorContext)] = typeof(TImplementation);
	}

	public void AddVisitor<TVisitorContext>(IVisitor<object> implementation)
		where TVisitorContext : ParserRuleContext
	{
		_visitors[typeof(TVisitorContext)] = implementation;
	}

	public IVisitor<object> GetVisitor<TVisitorContext>() where TVisitorContext : ParserRuleContext
	{
		if (!_instanced)
		{
			InstanceAll();
		}
		if (_visitors.ContainsKey(typeof(TVisitorContext)))
			return _visitors[typeof(TVisitorContext)];
		return _visitor;
	}

	internal void InstanceAll()
	{
		_instanced = true;
		var registeredType = _types.Values.Distinct();
		var instances = registeredType.Select(
			t => (IVisitor<object>)ActivatorUtilities.CreateInstance(_serviceProvider, t)
		).ToArray();

		var group = _types.GroupBy(T => T.Value).Zip(instances);

		foreach (var (first, second) in group)
		{
			foreach (var type in first)
			{
				_visitors[type.Key] = second;
			}
		}

		foreach (var visitor in instances)
			visitor.Configure(_serviceProvider);
	}
}
