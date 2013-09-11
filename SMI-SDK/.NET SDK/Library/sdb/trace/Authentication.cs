using System.Runtime.Serialization;

namespace pt.sapo.sdb.trace
{
	[DataContract(Namespace = TracingContractConstants.Namespace)]
	public class Authentication
	{
		[DataMember(EmitDefaultValue = false)]
		public string OnBehalf { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string Username { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public bool Succeeded { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string ClientPrimaryId { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string ClientContainer { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string ClientCredentialStore { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string Origin { get; set; }
	}
}