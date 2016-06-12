using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Quizer.Domain.Attributes;
using Quizer.Domain.Contracts;

namespace Quizer.Domain
{
    [DbCollection("students")]
    [JsonObject(MemberSerialization.OptIn)]
    public class Student : User, IDocument
    {
        [JsonProperty("birthDate")]
        public DateTimeOffset BirthDate { get; set; }

        [JsonProperty("bookNumber")]
        public string BookNumber { get; set; }

        [Required]
        [JsonProperty("group")]
        public string Group { get; set; }

        [Range(0, 100)]
        [JsonProperty("grade")]
        public int Grade { get; set; }

        [JsonProperty("testIds")]
        public List<Guid> TestIds { get; set; }
    }
}