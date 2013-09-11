using System.Runtime.Serialization;
using System;

namespace pt.sapo.sdb.trace
{
	[DataContract(Namespace = TracingContractConstants.Namespace)]
	public class TraceOccurrenceDate
	{
		[DataMember]
		public DateTime Date { get; set; }

		[DataMember]
		public int Count { get; set; }
	}
}
