using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace pt.sapo.sdb.trace
{
	[DataContract(Namespace = TracingContractConstants.Namespace)]
	public class Activity
	{
		[DataMember]
		public Guid Id { get; set; }

		[DataMember]
		public List<ActivityTraceOccurrence> Occurrences { get; set; }
	}

	[DataContract(Namespace = TracingContractConstants.Namespace)]
	public class ActivityTraceOccurrence
	{
		[DataMember]
		public Guid Id { get; set; }

		[DataMember]
		public DateTime Start { get; set; }

		[DataMember]
		public Service Service { get; set; }

		[DataMember]
		public List<ActivityTraceOccurrence> Children { get; set; }
	}
}