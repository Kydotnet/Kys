using System.CommandLine;
using System.CommandLine.Help;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using KYLib.Modding;
using Kys.Runtime;

namespace Kys;

internal class CommandLine
{
	public static int Main(string[] args)
	{
		Mod.EnableAutoLoads();
		/*Swm.Enabled = true;

		Swm.Step("Test");

		var n = KyObject.FromValue(0.0);
		var total = KyObject.FromValue(0.0);
		var val = KyObject.FromValue(1000000.0);

		while(CompilerCompare(ref n, val))
		{
			total = total.Aditive(true, n);
		}

		Console.WriteLine(total.ToCsharp());
		Swm.Step("Test");
		return 0;*/

		var showTimes = new Option<bool>(
				new[]
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

	private static bool CompilerCompare(ref IKyObject n, IKyObject val)
	{
		var dev =  n.Equality(false, val).Equals(KyObject.True);
		n = n.UnitaryOperation(true);
		return dev;
	}
	 
	// ReSharper disable once UnusedMember.Local
	static HelpBuilder GetHelpBuilder()
	{
		var dev = new HelpBuilder(LocalizationResources.Instance);
		return dev;
	}

	static string? Validatecommand(CommandResult symbolResult)
	{
		return null;
	}

	public static void KysCommand(InvocationContext ctx, FileInfo info, bool time)
	{
		ctx.ExitCode = Startup.Entry(info.FullName, time);
	}
}
