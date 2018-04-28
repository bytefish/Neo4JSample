// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Neo4j.Driver.V1;

namespace Neo4JSample.Settings
{
    public class ConnectionSettings : IConnectionSettings
    {
        public string Uri { get; private set; }

        public IAuthToken AuthToken { get; private set; }

        public ConnectionSettings(string uri, IAuthToken authToken)
        {
            Uri = uri;
            AuthToken = authToken;
        }

        public static ConnectionSettings CreateBasicAuth(string uri, string username, string password)
        {
            return new ConnectionSettings(uri, AuthTokens.Basic(username, password));
        }
    }
}
