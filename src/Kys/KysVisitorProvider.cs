using Antlr4.Runtime;
using Kys.Interpreter;
using Kys.Parser;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Kys;

public class KysVisitorProvider : IVisitorProvider
{
	IDictionary<Type, IKysParserVisitor<object>> _visitors = new ConcurrentDictionary<Type, IKysParserVisitor<object>>();

	IDictionary<Type, Type> _types = new ConcurrentDictionary<Type,Type>();

	IKysParserVisitor<object> _visitor = new KysParserBaseVisitor<object>();

	IServiceProvider serviceProvider;

	public KysVisitorProvider(IServiceProvider serviceProvider)
	{
		this.serviceProvider = serviceProvider;
	}

	public void AddVisitor<VisitorContext, Implementation>()
		where VisitorContext : ParserRuleContext
		where Implementation : IKysParserVisitor<object>
	{
		_types[typeof(VisitorContext)] = typeof(Implementation);
	}

	public void AddVisitor<VisitorContext, Implementation>(Implementation implementation)
		where VisitorContext : ParserRuleContext
		where Implementation : IKysParserVisitor<object>
	{
		_visitors[typeof(VisitorContext)] = implementation;
	}

	public IKysParserVisitor<dynamic> GetVisitor<VisitorContext>() where VisitorContext : ParserRuleContext
	{
		if(_visitors.ContainsKey(typeof(VisitorContext)))
			return _visitors[typeof(VisitorContext)];
		else if (_types.ContainsKey(typeof(VisitorContext)))
		{
			_visitors[typeof (VisitorContext)] = (IKysParserVisitor<object>)ActivatorUtilities.CreateInstance(serviceProvider, _types[typeof(VisitorContext)]);
			_types.Remove(typeof (VisitorContext));
			return _visitors[typeof(VisitorContext)];
		}

		return _visitor;
	}
}
