using System.ComponentModel.DataAnnotations;

namespace Quizer.Domain
{
    public class Option
    {
        [Required]
        public string Id { get; set; }

        public string Text { get; set; }

        public bool IsCorrect { get; set; }
    }
}