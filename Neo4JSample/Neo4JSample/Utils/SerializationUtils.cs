using Neo4JSample.Model;
using Neo4JSample.Serialization;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Neo4JSample.Utils
{
    public class SerializationUtils
    {
        public static Dictionary<string, string> AsDictionary(object source)
        {
            var json = JsonConvert.SerializeObject(source);

            var res =  JsonConvert.DeserializeObject<Dictionary<string, string>>(json, new JsonConverter[] { new CustomDictionaryConverter() });

            return res;
        }
    }
}
