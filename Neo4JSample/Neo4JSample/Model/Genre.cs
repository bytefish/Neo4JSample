using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neo4JSample.Model
{
    public class Genre
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
