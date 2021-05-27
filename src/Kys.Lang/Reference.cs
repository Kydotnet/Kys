namespace Kys.Lang
{
	public sealed class Reference
	{
		public string Name { get; init; }

		public Scope Source { get; init; }

		public dynamic Value
		{
			get => Source[Name];
			set => Source[Name] = value;
		}
	}
}