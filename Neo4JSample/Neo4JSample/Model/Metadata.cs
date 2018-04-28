using Neo4JSample.Serialization;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Neo4JSample.Model
{
    public class Metadata
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