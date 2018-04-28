// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Neo4JSample.Serializer;
using Neo4JSample.Serializer.Converters;
using Newtonsoft.Json;

namespace Neo4JSample.Model
{
    public class Metadata
    {
        [JsonProperty("movie")]
        [JsonConverter(typeof(CustomDictionaryConverter))]
        public Movie Movie { get; set; }

        [JsonProperty("director")]
        [JsonConverter(typeof(CustomDictionaryConverter))]
        public Person Director { get; set; }

        [JsonProperty("genres")]
        [JsonConverter(typeof(CustomDictionaryConverter))]
        public IList<Genre> Genres { get; set; }

        [JsonProperty("cast")]
        [JsonConverter(typeof(CustomDictionaryConverter))]
        public IList<Person> Cast { get; set; }
    }
}