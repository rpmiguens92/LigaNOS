using System.ComponentModel.DataAnnotations;

namespace LigaNOS.Data.Entities
{
    public class Country : IEntity
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characters.")]
        public string Name { get; set; }
    }
}
