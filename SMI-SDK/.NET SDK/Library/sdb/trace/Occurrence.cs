using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace pt.sapo.sdb.trace
{
	[DataContract(Namespace = TracingContractConstants.Namespace)]
	public class TraceData : IEntryContainer
	{
		[DataMember(EmitDefaultValue = false)]
		public Authentication Authentication { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public IList<Entry> Entries { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public IList<Message> Messages { get; set; }
	}

	[DataContract(Namespace = TracingContractConstants.Namespace)]
	public class Occurrence : TraceData
	{
		[DataMember(EmitDefaultValue = false)]
		public long TenantId { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string TraceId { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public Guid Id { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public Guid ActivityId { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public DateTime Start { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public DateTime End { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public Server Server { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public Client Client { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public Service Service { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public OccurrenceSummary Summary { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public Guid? ParentOccurrenceId { get; set; }
	}
}