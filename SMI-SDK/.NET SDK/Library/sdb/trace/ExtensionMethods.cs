using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pt.sapo.sdb.trace
{
    static class ExtensionMethods
    {
        public static NameValueCollection ToNameValueCollection(this IEnumerable<KeyValuePair<String, Object>> dictionary)
        {
            var result = new NameValueCollection();
            foreach (var nameValuePair in dictionary.Select(e => 
                new NameValuePair {
                    Name = e.Key,
                    ValuePairs = e.Value is IDictionary<String, Object> ? ToNameValueCollection(e.Value as IDictionary<String, Object>) : null,
                    ValueString = (e.Value != null)? e.Value.ToString() : ""
                }))
            {
                result.Add(nameValuePair);
            }
            return result;
        }
    }
}
