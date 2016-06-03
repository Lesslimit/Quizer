using System.ComponentModel.DataAnnotations;

namespace Quizer.Domain
{
    public class Option
    {
        [Required]
        public int Id { get; set; }

        public string Title { get; set; }

        public bool IsCorrect { get; set; }
    }
}