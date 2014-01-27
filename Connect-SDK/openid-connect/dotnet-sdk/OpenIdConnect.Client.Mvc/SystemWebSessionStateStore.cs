using System.Web;
using System.Web.SessionState;
using OpenIdConnect.Client.Common;

namespace OpenIdConnect.Client.Mvc
{
    /// <summary>
    /// A System.Web specific Session State Stores
    /// </summary>
    public class SystemWebSessionStateStore : ISessionStateStore
    {
        /// <summary>
        /// Gets or sets an entry in the user session state store
        /// </summary>
        /// <param name="index">The name/key of the entry</param>
        /// <returns>An object with the entry value</returns>
        public object this[string index]
        {
            get { return SessionState[index]; }
            set { SessionState[index] = value; }
        }

        /// <summary>
        /// Creates a new SystemWebSessionStateStore using the session store from HttpContext.Current.
        /// </summary>
        public SystemWebSessionStateStore()
        {
        }

        /// <summary>
        /// Creates a new SystemWebSessionStateStore using the given session state store.
        /// </summary>
        /// <param name="sessionState">The session state store to use internally</param>
        public SystemWebSessionStateStore(HttpSessionState sessionState)
        {
            _sessionState = sessionState;
        }

        private HttpSessionState _sessionState;
        private HttpSessionState SessionState
        {
            get
            {
                if (_sessionState == null)
                {
                    _sessionState = HttpContext.Current.Session;
                }

                return _sessionState;
            }
        }
    }
}