using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using GrEmit;

namespace Kys.Library;

partial class FunctionRegister
{
	static readonly MethodInfo _Wait = ((Action)Task.CompletedTask.Wait).Method;

	static MethodInfo IsAsync(MethodInfo method)
	{
		if (method.GetCustomAttribute<AsyncStateMachineAttribute>() == null && !method.ReturnType.IsAssignableTo(typeof(Task)))
			return method;
		if (method.ReturnType == typeof(void))
			throw new NotSupportedException("async void is not supported");
		return GenerateMethod(method);
	}

	static MethodInfo GenerateMethod(MethodInfo met)
	{
		var parameters = met.GetParameters().Select(p => p.ParameterType).ToArray();
		var returnType = met.ReturnType == typeof(Task) ? typeof(void) : met.ReturnType.GenericTypeArguments[0];
		var mb = new DynamicMethod(met.Name, returnType, parameters);

		return GenerateFor(mb, met, met.GetParameters(), met.ReturnType);
	}

	static MethodInfo GenerateFor(DynamicMethod mb, MethodInfo met, ParameterInfo[] parameterInfos, Type returnType)
	{
		using var il = new GroboIL(mb);
		var loc0 = il.DeclareLocal(returnType);
		for (var i = 0; i < parameterInfos.Length; i++)
			il.Ldarg(i);
		il.Call(met);
		il.Stloc(loc0);
		il.Ldloc(loc0);
		il.Call(_Wait);
		if (met.ReturnType != typeof(Task))
		{
			il.Ldloc(loc0);
			il.Call(returnType.GetProperty("Result")?.GetGetMethod());
		}
		il.Ret();
		return mb;
	}
}
