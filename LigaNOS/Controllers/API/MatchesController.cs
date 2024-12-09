using LigaNOS.Data.DTO;
using LigaNOS.Data;
using LigaNOS.Data.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using LigaNOS.Data.Entities;

namespace LigaNOS.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchesController : ControllerBase
    {
        private readonly DataContext _context;

        public MatchesController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var matches = await _context.Matches
                .Include(m => m.HomeClub)
                .Include(m => m.AwayClub)
                .ToListAsync();

            var result = matches.Select(m => new MatchDto
            {
                Id = m.Id,
                HomeClub = m.HomeClub?.Name,
                AwayClub = m.AwayClub?.Name,
                HomeGoals = m.HomeGoals,
                AwayGoals = m.AwayGoals,
                MatchDay = m.MatchDay,
                MatchTime = m.MatchTime,
                Stadium = m.Stadium
            });

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var match = await _context.Matches
                .Include(m => m.HomeClub)
                .Include(m => m.AwayClub)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (match == null)
            {
                return NotFound(new { message = "Match not found." });
            }

            var matchDto = new MatchDto
            {
                Id = match.Id,
                HomeClub = match.HomeClub?.Name,
                AwayClub = match.AwayClub?.Name,
                HomeGoals = match.HomeGoals,
                AwayGoals = match.AwayGoals,
                MatchDay = match.MatchDay,
                MatchTime = match.MatchTime,
                Stadium = match.Stadium
            };

            return Ok(matchDto);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] MatchDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var homeClub = await _context.Clubs.FirstOrDefaultAsync(c => c.Name == model.HomeClub);
            var awayClub = await _context.Clubs.FirstOrDefaultAsync(c => c.Name == model.AwayClub);

            if (homeClub == null || awayClub == null)
            {
                return NotFound(new { message = "One or both clubs not found." });
            }

            // Converte MatchDto para Match
            var match = new Match
            {
                HomeClubId = homeClub.Id,
                AwayClubId = awayClub.Id,
                HomeGoals = model.HomeGoals,
                AwayGoals = model.AwayGoals,
                MatchDay = model.MatchDay,
                MatchTime = model.MatchTime,
                Stadium = model.Stadium
            };

            // Adiciona ao contexto
            _context.Matches.Add(match);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = match.Id }, model);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [FromBody] MatchDto model)
        {
            if (id != model.Id)
            {
                return BadRequest(new { message = "Match ID mismatch." });
            }

            var match = await _context.Matches.FindAsync(id);
            if (match == null)
            {
                return NotFound(new { message = "Match not found." });
            }

            match.HomeGoals = model.HomeGoals;
            match.AwayGoals = model.AwayGoals;
            match.MatchDay = model.MatchDay;
            match.MatchTime = model.MatchTime;
            match.Stadium = model.Stadium;

            _context.Matches.Update(match);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var match = await _context.Matches.FindAsync(id);
            if (match == null)
            {
                return NotFound(new { message = "Match not found." });
            }

            if (match.MatchDay.Date < DateTime.Now.Date)
            {
                return BadRequest(new { message = "Cannot delete a match that has already been played." });
            }

            _context.Matches.Remove(match);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

