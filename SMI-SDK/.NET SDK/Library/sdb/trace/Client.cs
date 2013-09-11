using System.Runtime.Serialization;

namespace pt.sapo.sdb.trace
{
	[DataContract(Namespace = TracingContractConstants.Namespace)]
	public class Client
	{
		[DataMember(EmitDefaultValue = false)]
		public string Address { get; set; }
	}
}