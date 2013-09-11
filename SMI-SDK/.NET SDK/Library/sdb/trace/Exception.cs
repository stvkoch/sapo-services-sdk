using System.Runtime.Serialization;

namespace pt.sapo.sdb.trace
{
	[DataContract(Namespace = TracingContractConstants.Namespace)]
	public class Exception
	{
		[DataMember(EmitDefaultValue = false)]
		public string Type { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string StackTrace { get; set; }
	}
}