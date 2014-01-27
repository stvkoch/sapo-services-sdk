namespace OpenIdConnect.Client.Common.Exceptions
{
    /// <summary>
    /// Represents a mismatch when comparing a stored state with the returned state given by a callback URI.
    /// </summary>
    public class InvalidStateException : OpenIdConnectException
    {
        /// <summary>
        /// Creates a new InvalidStateException with the default message.
        /// </summary>
        public InvalidStateException()
            : base("OpenId Connect state mismatch. This could be an attempt to perform an CSRF attack on your application.")
        {
        }
    }
}