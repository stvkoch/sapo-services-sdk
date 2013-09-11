using System.Runtime.Serialization;

namespace pt.sapo.sdb.trace
{
	[DataContract(Namespace = TracingContractConstants.Namespace)]
	public class OccurrenceSummary
	{
		[DataMember]
		public string Result { get; set; }

		[DataMember]
		public bool Success { get; set; }
	}
}
