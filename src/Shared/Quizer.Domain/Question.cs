using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Quizer.Domain
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Question
    {
        [JsonProperty(Required = Required.DisallowNull)]
        public Guid Id { get; set; }

        [JsonProperty]
        public string Title { get; set; }

        [JsonProperty]
        public IList<Option> Options { get; set; }
    }
}