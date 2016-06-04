using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace Quizer.Domain
{
    public abstract class User
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public  string MiddleName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string Email { get; set; }
        
    }
}