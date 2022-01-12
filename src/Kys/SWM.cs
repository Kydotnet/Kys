using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Kys
{
	internal class SWM
	{
		static IDictionary<string, Stopwatch> sws = new Dictionary<string, Stopwatch>();

		public static bool Enabled = false;

		public static void Start(string id)
		{
			if (!Enabled) return;
			if (sws.ContainsKey(id))
			{
				sws[id].Restart();
			}
			else
			{
				sws[id] = Stopwatch.StartNew();
			}
		}

		public static void Stop(string id)
		{
			if (!Enabled) return;
			if (sws.ContainsKey(id))
			{
				var sw = sws[id];
				Console.WriteLine("{0} in {1}ms", id, sw.ElapsedMilliseconds);
				sws.Remove(id);
				sw.Stop();
			}
		}

		public static void Step(string id)
		{
			if (!Enabled) return;
			if (sws.ContainsKey(id))
			{
				Stop(id);
			}
			else
			{
				Start(id);
			}
		}
	}
}
