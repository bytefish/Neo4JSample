// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Newtonsoft.Json;

namespace Neo4JSample.Model
{
    public class Genre
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
