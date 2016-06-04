using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Quizer.Domain
{
    public class Student : User
    {
        public DateTimeOffset BirthDate { get; set; }

        public string BookNumber { get; set; }

        [Required]
        public string Group { get; set; }

        [Range(0, 100)]
        public int Grade { get; set; }

        public List<Guid> TestId { get; set; }
    }
}