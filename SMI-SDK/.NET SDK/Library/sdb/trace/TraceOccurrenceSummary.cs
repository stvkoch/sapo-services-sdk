using System;
using System.Runtime.Serialization;

namespace pt.sapo.sdb.trace
{
	[DataContract(Namespace = TracingContractConstants.Namespace)]
	public class TraceOccurrenceSummary
	{
		[DataMember]
		public Guid Id { get; set; }

		[DataMember]
		public DateTime Date { get; set; }

		[DataMember]
		public Server Server { get; set; }

		[DataMember]
		public Client Client { get; set; }

		[DataMember]
		public OccurrenceSummary Summary { get; set; }
	}
}