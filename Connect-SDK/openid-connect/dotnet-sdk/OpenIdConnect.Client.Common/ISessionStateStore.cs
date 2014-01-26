namespace OpenIdConnect.Client.Common
{
    /// <summary>
    /// Represents a store where the protocol state info will be stored.
    /// This store should be attached to the user's session.
    /// </summary>
    public interface ISessionStateStore
    {
        /// <summary>
        /// An indexer that enables read and store on the user's session
        /// </summary>
        /// <param name="index">the store entry key</param>
        object this[string index] { get; set; }
    }
}