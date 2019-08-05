using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace TeamCityImageChecker.RegistryJson
{
    [DataContract]
    public class TagsList
    {
        [DataMember]
        [JsonProperty("name")]
        public string Name { get; set; }

        [DataMember]
        [JsonProperty("tags")]
        public List<string> Tags { get; set; }
    }
}
