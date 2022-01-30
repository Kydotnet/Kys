using KYLib.Modding;
using KYLib.Utils;
using System.Resources;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Globalization;
using System.IO;

namespace Kys;

internal class CommandLine
{
	public static int Main(string[] args)
	{
		Mod.EnableAutoLoads();

		var showTimes = new Option<bool>(
				new string[]
				{
					"--show-time",
					"-t"
				},
				() => false,
				"Show Kys times like configuration time, execution time, etc."
			);

		var kysFile = new Argument<FileInfo>("Kys File", "Kys file to run")
			.ExistingOnly();

		var KysRoot = new RootCommand
		{
			kysFile,
			showTimes
		};

		KysRoot.Description = "Execute a kys file with the KyScript interpreter";

		KysRoot.SetHandler<InvocationContext, FileInfo, bool>(KysCommand, kysFile, showTimes);

		return KysRoot.Invoke("-h");

	}

	public static void KysCommand(InvocationContext ctx, FileInfo info, bool time)
	{
		ctx.ExitCode = Startup.Entry(info.FullName, time);
	}
}
