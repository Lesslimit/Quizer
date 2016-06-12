using Newtonsoft.Json;

namespace Quizer.Api.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class SignInModel
    {
        [JsonProperty]
        public string Email { get; set; }

        [JsonProperty]
        public string Password { get; set; }
    }
}