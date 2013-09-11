using System.Runtime.Serialization;

namespace pt.sapo.sdb.trace
{
	[DataContract(Namespace = TracingContractConstants.Namespace)]
	public class NameValuePair
	{
		[DataMember]
		public string Name { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string ValueString { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public NameValueCollection ValuePairs { get; set; }
	}
}