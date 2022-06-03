using System.Diagnostics;
namespace Kys;

internal class Swm
{
	readonly static IDictionary<string, Stopwatch> _Sws = new Dictionary<string, Stopwatch>();

	public static bool Enabled = false;

	public static void Start(string id)
	{
		if (!Enabled) return;
		if (_Sws.ContainsKey(id))
		{
			_Sws[id].Restart();
		}
		else
		{
			_Sws[id] = Stopwatch.StartNew();
		}
	}

	public static void Stop(string id)
	{
		if (!Enabled) return;
		if (_Sws.ContainsKey(id))
		{
			var sw = _Sws[id];
			Console.WriteLine("{0} in {1}ms", id, sw.ElapsedMilliseconds);
			_Sws.Remove(id);
			sw.Stop();
		}
	}

	public static void Step(string id)
	{
		if (!Enabled) return;
		if (_Sws.ContainsKey(id))
		{
			Stop(id);
		}
		else
		{
			Start(id);
		}
	}
}
