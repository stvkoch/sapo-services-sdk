
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
namespace pt.sapo.sdb.trace
{
    [JsonConverter(typeof(StringEnumConverter))]
	public enum Severity
	{
		Debug,
		Info,
		Warn,
		Error,
		Fatal,
	}
}