using System.CommandLine;
using System.CommandLine.Binding;
using System.CommandLine.Help;
using System.CommandLine.Invocation;
using System.IO;
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

		var kysRoot = new RootCommand
		{
			kysFile,
			kysArgs,
			showTimes
		};

		kysRoot.TreatUnmatchedTokensAsErrors = false;

		kysRoot.Description = "Execute a kys file with the KyScript interpreter";

		kysRoot.SetHandler<InvocationContext, FileInfo, bool>(KysCommand, kysFile, showTimes);

		kysRoot.AddValidator(Validatecommand);

		var a = kysRoot.Invoke(args);
		return a;
	}

	static HelpBuilder GetHelpBuilder(BindingContext arg)
	{
		var dev = new HelpBuilder(LocalizationResources.Instance);

		return dev;
	}

	static string? Validatecommand(System.CommandLine.Parsing.CommandResult symbolResult)
	{
		return null;
	}

	public static void KysCommand(InvocationContext ctx, FileInfo info, bool time)
	{
		ctx.ExitCode = Startup.Entry(info.FullName, time);
	}
}
