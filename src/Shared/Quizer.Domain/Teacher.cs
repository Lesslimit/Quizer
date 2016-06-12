using Newtonsoft.Json;
using Quizer.Domain.Attributes;

namespace Quizer.Domain
{
    [DbCollection("teachers")]
    [JsonObject(MemberSerialization.OptIn)]
    public class Teacher: User
    {
    }
}