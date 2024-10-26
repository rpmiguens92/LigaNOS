using System.ComponentModel.DataAnnotations;

namespace LigaNOS.Models
{
    public class ContactViewModel
    {

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Subject is required.")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Message is required.")]
        public string Message { get; set; }
    }
}
