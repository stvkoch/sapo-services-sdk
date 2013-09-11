using System;
using System.Runtime.Serialization;

namespace pt.sapo.sdb.trace
{
	[DataContract(Namespace = TracingContractConstants.Namespace)]
	public class Server
	{
		[DataMember(EmitDefaultValue = false)]
		public string Name { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string Address { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public DateTime? DeployDate { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string Version { get; set; }
	}
}