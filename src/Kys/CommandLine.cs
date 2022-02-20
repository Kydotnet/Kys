using System;
using System.CommandLine;
using System.CommandLine.Binding;
using System.CommandLine.Builder;
using System.CommandLine.Help;
using System.CommandLine.Invocation;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Threading.Tasks;
using KYLib.Extensions;
using KYLib.Modding;

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

		var kysArgs = new Argument<string[]>("Kys Arguments", "Command line args passed to kys")
		{
			Arity = ArgumentArity.ZeroOrMore
		};

		var KysRoot = new RootCommand
		{
			kysFile,
			kysArgs,
			showTimes
		};

		KysRoot.TreatUnmatchedTokensAsErrors = false;

		KysRoot.Description = "Execute a kys file with the KyScript interpreter";

		KysRoot.SetHandler<InvocationContext, FileInfo, bool>(KysCommand, kysFile, showTimes);

		KysRoot.AddValidator(validatecommand);

		var a = KysRoot.Invoke(args);
		return a;
	}

	private static HelpBuilder getHelpBuilder(BindingContext arg)
	{
		var dev = new HelpBuilder(LocalizationResources.Instance);

		return dev;
	}

	private static string? validatecommand(System.CommandLine.Parsing.CommandResult symbolResult)
	{
		return null;
	}

	public static void KysCommand(InvocationContext ctx, FileInfo info, bool time)
	{
		ctx.ExitCode = Startup.Entry(info.FullName, time);
	}
}
