using LigaNOS.Data.DTO;
using LigaNOS.Data.Entities;
using LigaNOS.Data;
using LigaNOS.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LigaNOS.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatsController : ControllerBase
    {
        private readonly IStatRepository _statRepository;
        private readonly DataContext _context;

        public StatsController(IStatRepository statRepository, DataContext context)
        {
            _statRepository = statRepository;
            _context = context;
        }

        // GET: api/stats
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var stats = await _context.Stats
                .Include(s => s.Match)
                .Include(s => s.HomeClub)
                .Include(s => s.AwayClub)
                .ToListAsync();

            var result = stats.Select(s => new StatDto
            {
                Id = s.Id,
                MatchId = s.MatchId,
                HomeClub = s.HomeClub.Name,
                AwayClub = s.AwayClub.Name,
                HomeClubGoals = s.HomeClubGoals,
                AwayClubGoals = s.AwayClubGoals,
                HomeClubGoalsConceded = s.HomeClubGoalsConceded,
                AwayClubGoalsConceded = s.AwayClubGoalsConceded,
                HomeClubPoints = s.HomeClubPoints,
                AwayClubPoints = s.AwayClubPoints,
                MatchDay = s.Match.MatchDay.ToString("yyyy-MM-dd")
            });

            return Ok(result);
        }

        // GET: api/stats/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var stat = await _context.Stats
                .Include(s => s.Match)
                .Include(s => s.HomeClub)
                .Include(s => s.AwayClub)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (stat == null)
            {
                return NotFound(new { message = "Stat not found." });
            }

            var statDto = new StatDto
            {
                Id = stat.Id,
                MatchId = stat.MatchId,
                HomeClub = stat.HomeClub.Name,
                AwayClub = stat.AwayClub.Name,
                HomeClubGoals = stat.HomeClubGoals,
                AwayClubGoals = stat.AwayClubGoals,
                HomeClubGoalsConceded = stat.HomeClubGoalsConceded,
                AwayClubGoalsConceded = stat.AwayClubGoalsConceded,
                HomeClubPoints = stat.HomeClubPoints,
                AwayClubPoints = stat.AwayClubPoints,
                MatchDay = stat.Match.MatchDay.ToString("yyyy-MM-dd")
            };

            return Ok(statDto);
        }

        // POST: api/stats/update
        [HttpPost("update")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateStats()
        {
            var matches = _context.Matches
                .Include(m => m.HomeClub)
                .Include(m => m.AwayClub)
                .ToList();

            var statsList = new List<Stat>();

            foreach (var match in matches)
            {
                statsList.Add(new Stat
                {
                    MatchId = match.Id,
                    HomeClubId = match.HomeClub.Id,
                    AwayClubId = match.AwayClub.Id,
                    HomeClubGoals = match.HomeGoals,
                    AwayClubGoals = match.AwayGoals,
                    Wins = match.HomeGoals > match.AwayGoals ? 1 : 0,
                    Draws = match.HomeGoals == match.AwayGoals ? 1 : 0,
                    Losses = match.HomeGoals < match.AwayGoals ? 1 : 0,
                    Points = match.HomeGoals > match.AwayGoals ? 3 : match.HomeGoals == match.AwayGoals ? 1 : 0
                });

                statsList.Add(new Stat
                {
                    MatchId = match.Id,
                    HomeClubId = match.AwayClub.Id,
                    AwayClubId = match.HomeClub.Id,
                    HomeClubGoals = match.AwayGoals,
                    AwayClubGoals = match.HomeGoals,
                    Wins = match.AwayGoals > match.HomeGoals ? 1 : 0,
                    Draws = match.AwayGoals == match.HomeGoals ? 1 : 0,
                    Losses = match.AwayGoals < match.HomeGoals ? 1 : 0,
                    Points = match.AwayGoals > match.HomeGoals ? 3 : match.AwayGoals == match.HomeGoals ? 1 : 0
                });
            }

            _context.Stats.AddRange(statsList);
            _context.SaveChanges();

            return Ok(new { message = "Statistics updated successfully." });
        }
    }
}
