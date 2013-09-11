using System.Runtime.Serialization;

namespace pt.sapo.sdb.trace
{
	[DataContract(Namespace = TracingContractConstants.Namespace)]
	public class Service
	{
		[DataMember(EmitDefaultValue = false)]
		public string Name { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string Operation { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public long? Strategy { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string StrategyXml { get; set; }
	}
}