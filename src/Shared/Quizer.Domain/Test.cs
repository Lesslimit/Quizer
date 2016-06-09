using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Quizer.Domain
{
    public class Test
    {

        [Required]
        public Guid Id { get; set; }

        [Range(0, 100)]
        public int Result { get; set; }

        public DateTimeOffset Time { get; set; }

        public DateTimeOffset StartTime { get; set; }

        public DateTimeOffset EndTime { get; set; }
    
        public bool IsComplete { get; set; }

        public List<Question> Questions { get; set; }

        public List<string> StudentsId { get; set; }
    }
}