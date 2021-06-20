namespace Kys.Lang
{
	public sealed class Reference
	{
		public string Name { get; init; }

		public IScope Source { get; init; }

		public dynamic Value
		{
			get => Source.GetVar(Name);
			set => Source.SetVar(Name, value);
		}
	}
}