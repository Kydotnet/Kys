namespace Kys.Parser;
partial class KysParser
{

	partial class ValueContext
	{
		string? _string;
		bool readString;
		bool hasString;

		bool? _bool;
		bool readBool;
		bool hasBool;

		double? _double;
		int? _int;
		bool readNumber;
		bool hasNumber;

		string? _Id;
		bool readId;
		bool hasId;

		public bool HasString
		{
			get
			{
				if(!readString)
				{
					readString = true;
					var node = STRING;
					if (node != null)
					{
						_string = node.GetText().Trim('"');
						hasString = true;
					}
					else
					{
						hasString = false;
					}
					
				}
				return hasString;
			}
		}

		public bool HasBool
		{
			get
			{
				if (!readBool)
				{
					readBool = true;
					var node = BOOL;
					if (node != null)
					{
						_bool = node.GetText().ToLower().Equals("true");
						hasBool = true;
					}
					else
					{
						hasBool = false;
					}

				}
				return hasBool;
			}
		}

		public bool HasNumber
		{
			get
			{
				if (!readNumber)
				{
					readNumber = true;
					var node = NUMBER;
					if (node != null)
					{
						var raw = node.GetText();
						if (int.TryParse(raw, out int retint))
						{
							IsInt = true;
							_int = retint;
						}
						else
						{
							IsInt = false;
							_double = double.Parse(raw);
						}

						hasNumber = true;
					}
					else
					{
						hasNumber = false;
					}

				}
				return hasNumber;
			}
		}

		public bool HasId
		{
			get
			{
				if (!readId)
				{
					readId = true;
					var node = ID;
					if (node != null)
					{
						_Id = node.GetText();
						hasId = true;
					}
					else
					{
						hasId = false;
					}

				}
				return hasId;
			}
		}

		public string String => _string!;

		public bool Bool => _bool.Value;

		public int Int => _int.Value;

		public double Double => _double.Value;

		public bool IsInt { get; private set; }

		public string Id => _Id!;

	}
}