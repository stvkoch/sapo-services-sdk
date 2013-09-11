using System;
using System.Runtime.Serialization;

namespace pt.sapo.sdb.trace
{
	[DataContract(Namespace = TracingContractConstants.Namespace)]
	public class TraceListItem
	{
		[DataMember]
		public string Url { get; set; }

		[DataMember]
		public string TraceId { get; set; }

		[DataMember]
		public DateTime Date { get; set; }

		[DataMember]
		public Guid LastOccurrenceId { get; set; }

		[DataMember]
		public string ServiceName { get; set; }

		[DataMember]
		public string OperationName { get; set; }

		[DataMember]
		public OccurrenceSummary Summary { get; set; }
	}
}