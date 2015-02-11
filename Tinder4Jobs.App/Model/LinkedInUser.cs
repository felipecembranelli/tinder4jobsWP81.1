using Newtonsoft.Json;

namespace Tinder4Jobs.Model
{
    public class LinkedinUser
    {
        //[JsonProperty("id")]
        //public string Id { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("headline")]
        public string HeadLine { get; set; }
    }
}