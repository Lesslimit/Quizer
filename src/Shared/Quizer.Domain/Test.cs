using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Quizer.Domain
{
    public class Test
    {

        [Required]
        public int Id { get; set; }

        public IList<Question> Questions { get; set; }

        [Range(0, 100)]
        public int Result { get; set; }

        public DateTime Time { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public bool IsComplete { get; set; }
    }
}