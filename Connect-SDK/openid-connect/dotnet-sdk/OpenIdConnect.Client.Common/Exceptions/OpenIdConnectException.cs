using System;

namespace OpenIdConnect.Client.Common.Exceptions
{
    /// <summary>
    /// The base class for OpenIdConnect-related exceptions
    /// </summary>
    public abstract class OpenIdConnectException : Exception
    {
        /// <summary>
        /// Creates an OpenIdConnectException with a custom message
        /// </summary>
        /// <param name="message"></param>
        protected OpenIdConnectException(string message)
            : base(message)
        {
        }
    }
}