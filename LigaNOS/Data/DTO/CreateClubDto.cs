using Microsoft.AspNetCore.Http;

namespace LigaNOS.Data.DTO
{
    public class CreateClubDto
    {
        public string Name { get; set; }
        public string Coach { get; set; }
        public string Stadium { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
