using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace pt.sapo.sdb.trace
{
	[DataContract(Namespace = TracingContractConstants.Namespace)]
	public class Message
	{
		[DataMember(EmitDefaultValue = false)]
		public Request Request { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public Response Response { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public NameValueCollection Headers { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public byte[] Body { get; set; }

		[JsonIgnore]
		public int BodyId { get; set; }
	}
}