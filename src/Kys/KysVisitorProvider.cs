using System.Collections.Concurrent;
using Antlr4.Runtime;
using Kys.Interpreter;
using Kys.Interpreter.Visitors;
using Kys.Runtime;
using Microsoft.Extensions.DependencyInjection;
namespace Kys;

public class KysVisitorProvider : IVisitorProvider
{
	readonly IDictionary<Type, IVisitor<IKyObject>> _visitors = new ConcurrentDictionary<Type, IVisitor<IKyObject>>();

	readonly IDictionary<Type, Type> _types = new ConcurrentDictionary<Type, Type>();

	readonly IVisitor<IKyObject> _visitor = new BaseVisitor<IKyObject>();

	readonly IServiceProvider _serviceProvider;

	bool _instanced;

	public KysVisitorProvider(IServiceProvider serviceProvider)
	{
		_serviceProvider = serviceProvider;
	}

	public void AddVisitor<TVisitorContext, TImplementation>()
		where TVisitorContext : ParserRuleContext
		where TImplementation : IVisitor<IKyObject>
	{
		_types[typeof(TVisitorContext)] = typeof(TImplementation);
	}

	public void AddVisitor<TVisitorContext>(IVisitor<IKyObject> implementation)
		where TVisitorContext : ParserRuleContext
	{
		_visitors[typeof(TVisitorContext)] = implementation;
	}

	public IVisitor<IKyObject> GetVisitor<TVisitorContext>() where TVisitorContext : ParserRuleContext
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
			t => (IVisitor<IKyObject>)ActivatorUtilities.CreateInstance(_serviceProvider, t)
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
