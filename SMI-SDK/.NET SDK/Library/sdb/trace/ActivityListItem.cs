using System;
using System.Runtime.Serialization;

namespace pt.sapo.sdb.trace
{
	[DataContract(Namespace = TracingContractConstants.Namespace)]
	public class ActivityListItem
	{
		[DataMember]
		public string Url { get; set; }

		[DataMember]
		public string ActivityId { get; set; }

		[DataMember]
		public DateTime Date { get; set; }

		[DataMember]
		public int NumberOfOccurrences { get; set; }

		[DataMember]
		public OccurrenceSummary FirstFail { get; set; }
	}
}