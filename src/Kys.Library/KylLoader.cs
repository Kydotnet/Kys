namespace Kys.Library;

public static class KylLoader
{
	static Type Type = typeof(KylLoader);

	public static void Load(KylContext context)
	{
		var cmd = context.ID().GetText().ToLower();
		var args = context.STRING().Select(s => s.GetText().Trim('"')).ToArray();
		switch (cmd)
		{
			case "gen":
				{
					KylGenerator.Generate(args);
					break;
				}
			default:
				throw new NotImplementedException();
		}
	}
}
