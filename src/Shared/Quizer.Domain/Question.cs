using System.ComponentModel.DataAnnotations;

namespace Quizer.Domain
{
    public class Question
    {
        [Required]
        public int Id { get; set; }

        public string Title { get; set; }

        public Option Options { get; set; }

        public Option CorrectAnswer { get; set; }
    }
}