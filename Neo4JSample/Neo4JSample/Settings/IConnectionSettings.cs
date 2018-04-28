// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Neo4j.Driver.V1;

namespace Neo4JSample.Settings
{
    public interface IConnectionSettings
    {
        string Uri { get; }

        IAuthToken AuthToken { get; }
    }
}
