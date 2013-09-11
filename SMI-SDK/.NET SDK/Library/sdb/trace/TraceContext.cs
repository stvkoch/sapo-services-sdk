using Newtonsoft.Json;
using PTCom.Applications.ESB.ApplicationBlocks.Utilities;
using System;
using System.Text;

namespace pt.sapo.sdb.trace
{
	public sealed class TraceContext
	{
		public Guid LinkedTraceId { get; set; }
		public Guid OwnerTraceId { get; set; }

		public override string ToString()
		{
			var json = JsonConvert.SerializeObject(this);
			return Encoding.UTF8.GetBytes(json).ToBase64UrlWithoutPadding();
		}

		public static TraceContext Parse(string value)
		{
			var json = Encoding.UTF8.GetString(value.FromBase64UrlWithoutPadding());
			return JsonConvert.DeserializeObject<TraceContext>(json);
		}
	}
}
