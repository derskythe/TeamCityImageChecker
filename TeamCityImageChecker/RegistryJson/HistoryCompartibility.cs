using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace TeamCityImageChecker.RegistryJson
{
    [DataContract]
    public class HistoryCompartibility
    {
        [DataMember]
        [JsonProperty("tag")]
        public string Tag { get; set; }

        [DataMember]
        [JsonProperty("container")]
        public string Container { get; set; }

        [DataMember]
        [JsonProperty("created")]
        public DateTime Created { get; set; }

        public override string ToString()
        {
            return $"{nameof(Tag)}: {Tag}, {nameof(Container)}: {Container}, {nameof(Created)}: {Created}";
        }
    }
}
