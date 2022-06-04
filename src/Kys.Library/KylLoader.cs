using Kys.Library.Kyl;
namespace Kys.Library;

internal static class KylLoader
{
	public static void Load(KylContext context, IContext targetContext)
	{
		var cmd = context.IDText.ToLower();
		var args = context.STRING().Select(s => s.GetText().Trim('"')).ToArray();
		switch (cmd)
		{
			case "gen":
				{
					KylGenerator.Generate(args, targetContext);
					break;
				}
			default:
				throw new NotImplementedException("Unknow kyl instruction");
		}
	}
}
