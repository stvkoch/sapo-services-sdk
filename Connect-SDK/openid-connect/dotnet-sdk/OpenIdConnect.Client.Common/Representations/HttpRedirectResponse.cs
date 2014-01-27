using System;

namespace OpenIdConnect.Client.Common.Representations
{
    /// <summary>
    /// Represents an HTTP Redirect Response (needed for framework abstraction)
    /// </summary>
    public class HttpRedirectResponse
    {
        /// <summary>
        /// The redirect location
        /// </summary>
        public Uri Location { get; set; }

        /// <summary>
        /// Creates a new HttpRedirectResponse, given a location as a string.
        /// The given string will be internally converted to an Uri object
        /// </summary>
        /// <param name="location">The redirect location, as a string</param>
        public HttpRedirectResponse(string location)
            : this(new Uri(location))
        {
        }

        /// <summary>
        /// Creates a new HttpRedirectResponse, given a location as an Uri.
        /// </summary>
        /// <param name="location">The redirect location as an Uri</param>
        public HttpRedirectResponse(Uri location)
        {
            Location = location;
        }
    }
}