using LigaNOS.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LigaNOS.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatsController : Controller
    {

        private readonly IStatRepository _statRepository;

        public StatsController(IStatRepository statRepository)
        {
           _statRepository = statRepository;
        }
        [HttpGet]
        public IActionResult GetStats()
        {  
            return Ok(_statRepository.GetAll());
        }
    }
}
