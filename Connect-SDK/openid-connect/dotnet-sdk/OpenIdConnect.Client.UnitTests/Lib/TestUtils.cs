using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenIdConnect.Client.UnitTests
{
    public class TestUtils
    {
        public static IDictionary<string, string> GetParsedQueryString(Uri uri)
        {
            string[] parts = uri.Query.Split(new[] { '?', '&' }, StringSplitOptions.RemoveEmptyEntries);
            
            return parts.Select(p =>
            {
                var nameAndValue = p.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);

                if (nameAndValue.Length == 1)
                {
                    // enable parameters without value
                    return new KeyValuePair<string, string>(nameAndValue[0], string.Empty);
                }

                if (nameAndValue.Length == 2)
                {
                    return new KeyValuePair<string, string>(nameAndValue[0], nameAndValue[1]);
                }

                // this should never happen
                throw new ArgumentException("Invalid query in the URI");
            })
                .ToDictionary(p => p.Key, p => p.Value);
        }
    }
}