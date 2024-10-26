using LigaNOS.Data;
using LigaNOS.Data.Entities;
using LigaNOS.Data.Repositories;
using LigaNOS.Helpers;
using LigaNOS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LigaNOS.Controllers
{
    [Authorize(Roles = "Admin, Emplo")]
    public class MatchesController : Controller
    {
        private readonly IMatchRepository _matchRepository;
        private readonly IUserHelper _userHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IClubRepository _clubRepository;
        private readonly ITeamService _teamService;
        private readonly IMatchGenerator _matchGenerator;

        public MatchesController(IMatchRepository matchRepository, 
            IUserHelper userHelper, IBlobHelper blobHelper, IConverterHelper converterHelper,
            IClubRepository clubRepository, IMatchGenerator matchGenerator)
        {
            _matchRepository = matchRepository;
            _matchGenerator = matchGenerator;
            _userHelper = userHelper;
            _blobHelper = blobHelper;
            _converterHelper = converterHelper;
            _clubRepository = clubRepository;
        }

        // GET: MatchesController
        public async Task<IActionResult> Index()
            {
            var matches = await _matchRepository.GetAll()
            .Include(m => m.HomeClub)
            .Include(m => m.AwayClub)
            .ToListAsync();

            return View(matches);
             }

        // GET: MatchesController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewModel("MatchNotFound");
            }

            var match = await _matchRepository.GetAll()
               .Include(m => m.HomeClub)
               .Include(m => m.AwayClub)
               .FirstOrDefaultAsync(m => m.Id == id.Value);

            if (match == null)
            {
                return new NotFoundViewModel("MatchNotFound");
            }

            return View(match);
        }

        // GET: MatchesController/Create
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        { 
            var match = await _matchGenerator.GenerateMatch();

            var matchViewModel = new MatchViewModel
            {
                HomeClub = match.HomeClub,
                AwayClub = match.AwayClub,
                Stadium = match.Stadium,
                HomeClubId = match.HomeClubId,  
                AwayClubId = match.AwayClubId,
                MatchDay = DateTime.Now,
            };

            return View(matchViewModel);
        }

        // POST: MatchesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MatchViewModel model)
        {

            if (ModelState.IsValid)
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "Account");
                }

               
                var homeClub = await _clubRepository.GetByIdAsync(model.HomeClubId);
                var awayClub = await _clubRepository.GetByIdAsync(model.AwayClubId);

                if (homeClub == null || awayClub == null)
                {
                    return NotFound("No clubs available for matches.");
                }

                var match = new Match
                {
                    HomeClubId = homeClub.Id,
                    AwayClubId = awayClub.Id,
                    HomeClub = homeClub,
                    AwayClub = awayClub,
                    Stadium = model.Stadium,
                    MatchDay = model.MatchDay,
                    MatchTime = model.MatchTime
                };

                match.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                
                await _matchRepository.CreateAsync(match);
                return RedirectToAction(nameof(Index));
            }

            return View(model);
            
        }

        // GET: MatchesController/Edit/5
 
        public async Task<IActionResult> Edit(int? id)
        {  
            if (id == null)
            {
                return new NotFoundViewModel("MatchNotFound");
            } 

            var match = await _matchRepository.GetAll()
                                .Include(m => m.HomeClub)
                                .Include(m => m.AwayClub)
                                .FirstOrDefaultAsync(m => m.Id == id.Value);

            if (match == null)
            {
                return new NotFoundViewModel("MatchNotFound");
            }
 
            if (match.HomeClub == null || match.AwayClub == null)
            {
                ModelState.AddModelError("", "Clubs not defined.");
                return View();
            }

            
            var model = new MatchViewModel
            {
                Id = match.Id,
                HomeClub = match.HomeClub.Name, 
                AwayClub = match.AwayClub.Name, 
                Stadium = match.Stadium,
                HomeGoals = match.HomeGoals,
                AwayGoals = match.AwayGoals,
                MatchDay = match.MatchDay,
                MatchTime = match.MatchTime
            };

            return View(model);
        }

        // POST: MatchesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MatchViewModel model)
        {
            if (id != model.Id)
            {
                return new NotFoundViewModel("MatchNotFound");
            }

            if (ModelState.IsValid)
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "Account");
                }

                try
                {
                    var match = await _matchRepository.GetByIdAsync(model.Id);
                    if (match == null)
                    {
                        return new NotFoundViewModel("MatchNotFound");
                    }

                    match.HomeClub = await _clubRepository.GetByIdAsync(model.HomeClubId);
                    match.AwayClub = await _clubRepository.GetByIdAsync(model.AwayClubId);
                    match.Stadium = model.Stadium;
                    match.MatchDay = model.MatchDay;
                    match.MatchTime = model.MatchTime;
                    match.HomeGoals = model.HomeGoals;
                    match.AwayGoals = model.AwayGoals;


                    match.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

                    await _matchRepository.UpdateAsync(match);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _matchRepository.ExistAsync(model.Id))
                    {
                        return new NotFoundViewModel("MatchNotFound");
                    }
                    else
                    {
                        throw;
                    }
                    
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: MatchesController/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewModel("MatchNotFound");
            }
            

            var match = await _matchRepository.GetAll()
              .Include(m => m.HomeClub)
              .Include(m => m.AwayClub)
              .FirstOrDefaultAsync(m => m.Id == id.Value);

            if (match == null)
            {
                return new NotFoundViewModel("MatchNotFound");
            }
            return View(match);
        }


        // POST: MatchesController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var match = await _matchRepository.GetByIdAsync(id);

            if (match == null)
            {
                return new NotFoundViewModel("MatchNotFound");
            }
            //delete only if the match is not played yet
            if (match.MatchDay.Date < DateTime.Now.Date)
            {
                ModelState.AddModelError(string.Empty, "Game already happened, for statistics reasons we can´t remove the results");
                return View(match);
            }
            await _matchRepository.DeleteAsync(match);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult MatchNotFound()
        {
            return View();
        }
    }
}
