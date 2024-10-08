using LigaNOS.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LigaNOS.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchesController : Controller
    {
        private readonly IMatchRepository _matchRepository;

        public MatchesController(IMatchRepository matchRepository)
        {
            _matchRepository = matchRepository;
        }

        [HttpGet]
        public IActionResult GetMatches()
        {
            return Ok(_matchRepository.GetAll());
        }

    }
}
