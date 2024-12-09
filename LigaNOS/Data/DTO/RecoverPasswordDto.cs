using System.ComponentModel.DataAnnotations;
 

namespace LigaNOS.Data.DTO
{
    public class RecoverPasswordDto
    {

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
