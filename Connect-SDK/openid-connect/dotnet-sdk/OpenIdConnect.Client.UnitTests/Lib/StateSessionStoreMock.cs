using System.Collections.Generic;
using OpenIdConnect.Client.Common;

namespace OpenIdConnect.Client.UnitTests
{
    public class StateSessionStoreMock : Dictionary<string, object>, ISessionStateStore
    {
    }
}