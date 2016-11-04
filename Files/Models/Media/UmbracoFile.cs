using Newtonsoft.Json;

namespace ExtendingUmbraco.Models.Media
{
    public class UmbracoFile
    {
        [JsonProperty("src")]
        public string Src { get; set; }
    }
}