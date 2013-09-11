using System.Collections.Generic;

namespace pt.sapo.sdb.trace
{
	public interface IEntryContainer
	{
		IList<Entry> Entries { get; set; }
	}
}