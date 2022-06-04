using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

var file = args[0];
var content = File.ReadAllText(file);
string open = "{";
string close = "}";

var terminalNodeRegex = new Regex("public ITerminalNode (\\w*)\\(\\) { return (.*) }");

var terminalNodes = terminalNodeRegex.Matches(content);


foreach (Match terminalNode in terminalNodes.DistinctBy(s => s.Groups[1].Value))
{
	Console.WriteLine("Replacing terminal node \"{0}\" ", terminalNode.Groups[1]);
	content = content.Replace(terminalNode.Value, GetNodeReplace(terminalNode.Value, terminalNode.Groups[1]));
}

var contextNodeRegex = new Regex("public (\\w+Context(\\[\\])?) (\\w*)\\(\\) {[\\n\\w\\s<>();]+}");

var contextNodes = contextNodeRegex.Matches(content);

foreach(Match match in contextNodes.DistinctBy(s => s.Groups[1] + " " + s.Groups[3]))
{
	Console.WriteLine("Replacing context \"{0} {1}\" ", match.Groups[1], match.Groups[3]);
	content = content.Replace(match.Value, GetContextReplace(match.Value, match.Groups[1].Value, CultureInfo.CurrentCulture.TextInfo.ToTitleCase(match.Groups[3].Value.ToLower()), match.Groups[3].Value));
}

File.WriteAllText(file, content);

string GetContextReplace(string value, string type, string name, string orig)
{
	var builder = new StringBuilder();
	
	builder.Append(value.Replace(orig + "()", "_" + orig + "()"));
	builder.Append(Environment.NewLine);

	builder.Append(
$"""
/* codigo generado para context {type} {orig} */

"""
);

	createPropWithBack(builder, name, "_" + orig + "()", type);

	return builder.ToString();
}

string GetNodeReplace(string value, Group group)
{
	var builder = new StringBuilder();

	builder.Append(value.Replace(group.Value + "()", "_" + group.Value + "()"));
	builder.Append(Environment.NewLine);
	

	builder.Append(
$"""
/* codigo generado para node {group.Value} */

"""
	);
	createPropWithBack(builder, group.Value, $"_{group.Value}()", "ITerminalNode");

	createPropWithBack(builder, group.Value + "Text", $"{group.Value}.GetText()", "string");

	return builder.ToString();
}

void createPropWithBack(StringBuilder builder, string name, string value, string type)
{
	builder.Append(
$"""
	bool read{name};
"""
	);

	builder.Append(
$"""
	{type} back{name};
"""
);

	builder.Append(
$"""
	public {type} {name}
"""
);
	builder.Append(
$"""

{open}
	get
	{open}
		if(!read{name})
		{open}
			read{name} = true;
			back{name} = {value};
		{close}
		return back{name};
	{close}
{close}

"""
);
}