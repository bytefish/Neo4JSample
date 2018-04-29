// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Neo4JSample.Model
{
    public class MovieInformation
    {
        [JsonProperty("movie")]
        public Movie Movie { get; set; }

        [JsonProperty("director")]
        public Person Director { get; set; }

        [JsonProperty("genres")]
        public IList<Genre> Genres { get; set; }

        [JsonProperty("cast")]
        public IList<Person> Cast { get; set; }
    }
}