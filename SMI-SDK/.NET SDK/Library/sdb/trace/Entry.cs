using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace pt.sapo.sdb.trace
{
	[DataContract(Namespace = TracingContractConstants.Namespace)]
	public class Entry : IEntryContainer
	{
		[DataMember(EmitDefaultValue = false)]
		public Severity? Severity { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string Description { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public double? Offset { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public double? Duration { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public int? MessageNumber { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public Exception Exception { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public NameValueCollection Properties { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public IList<Entry> Entries { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string LinkedTraceId { get; set; }
	}
}