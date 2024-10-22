using System.ComponentModel.DataAnnotations;

namespace LigaNOS.Models
{
    public class RecoverPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

    }
}
