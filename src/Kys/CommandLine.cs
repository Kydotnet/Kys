using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Globalization;
using System.IO;
using System.Resources;
using KYLib.Modding;
using KYLib.Utils;

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

		var kysArgs = new Argument<string>("Kys Arguments", "Command line args passed to kys")
		{
			Arity = ArgumentArity.ZeroOrMore
		};

		var KysRoot = new RootCommand
		{
			showTimes,
			kysFile,
			kysArgs
		};

		KysRoot.Description = "Execute a kys file with the KyScript interpreter";

		KysRoot.SetHandler<InvocationContext, FileInfo, bool>(KysCommand, kysFile, showTimes);

		return KysRoot.Invoke(args);

	}

	public static void KysCommand(InvocationContext ctx, FileInfo info, bool time)
	{
		ctx.ExitCode = Startup.Entry(info.FullName, time);
	}
}
