using System.Runtime.Serialization;

namespace pt.sapo.sdb.trace
{
	[DataContract(Namespace = TracingContractConstants.Namespace)]
	public class Request
	{
		[DataMember(EmitDefaultValue = false)]
		public string Verb { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public bool Secure { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string PathAndQuery { get; set; }
	}
}