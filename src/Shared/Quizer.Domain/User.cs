using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace Quizer.Domain
{
    public class User
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public MailAddress Email { get; set; }

        public DateTime BirthDate { get; set; }

        public int StudentsBookNumber { get; set; }

        [Required]
        public string Group { get; set; }

        [Range(0, 100)]
        public int Mark { get; set; }
    }
}