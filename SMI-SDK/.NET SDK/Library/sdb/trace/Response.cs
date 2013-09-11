using System.Runtime.Serialization;

namespace pt.sapo.sdb.trace
{
	[DataContract(Namespace = TracingContractConstants.Namespace)]
	public class Response
	{
		[DataMember(EmitDefaultValue = false)]
		public int StatusCode { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string ReasonPhrase { get; set; }
	}
}