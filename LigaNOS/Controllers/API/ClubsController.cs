using LigaNOS.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace LigaNOS.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClubsController : Controller
    {
        private readonly IClubRepository _clubRepository;

        public ClubsController(IClubRepository clubRepository)
        {
            _clubRepository = clubRepository;
        }
        [HttpGet]
        public IActionResult GetClubs()
        {
            return Ok(_clubRepository.GetAll());
           
        }
    }

}
