using LigaNOS.Data.Repositories;
using LigaNOS.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LigaNOS.Models;
using System.Linq;
using LigaNOS.Data.Entities;
using LigaNOS.Data;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
namespace LigaNOS.Controllers
{
    public class StatsController : Controller
    {
        private readonly IStatRepository _statRepository;
        private readonly IMatchRepository _matchRepository;
        private readonly IClubRepository _clubRepository;
        private readonly IUserHelper _userHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IBlobHelper _blobHelper; 
        private readonly DataContext _context;
        public StatsController(IStatRepository statRepository, IMatchRepository matchRepository, IClubRepository clubRepository, IUserHelper userHelper, IConverterHelper converterHelper, IBlobHelper blobHelper, DataContext dataContext)
        {
            _statRepository = statRepository;
            _matchRepository = matchRepository;
            _clubRepository = clubRepository;
            _userHelper = userHelper;
            _converterHelper = converterHelper;
            _blobHelper = blobHelper;
             
            _context = dataContext;
        }
        // GET: StatsController
        
        public ActionResult Index()
        {
            if (_matchRepository == null)
            {
                throw new InvalidOperationException("_matchRepository is not initialized.");
            }
            var matches = _matchRepository.GetAllWithUsers()
                .OfType<Match>()
                .Include(m => m.HomeClub)
                .Include(m => m.AwayClub)
                .Select(m => new MatchViewModel
                {
                    HomeClub = m.HomeClub.Name,
                    AwayClub = m.AwayClub.Name,
                    HomeGoals = m.HomeGoals,
                    AwayGoals = m.AwayGoals,
                    MatchDay = m.MatchDay,
                    MatchTime = m.MatchTime,
                    Stadium = m.Stadium
                })
                .ToList();
            var clubs = _clubRepository.GetAllWithUsers().OfType<Club>().ToList();
            var clubStats = new List<ClubStatViewModel>();
            if (clubStats == null)
            {
                throw new InvalidOperationException("No club stats found.");
            }
            foreach (var club in clubs)
            {
                var matchesForClub = _context.Matches
                    .Where(m => m.HomeClub.Id == club.Id || m.AwayClub.Id == club.Id)
                    .ToList();

                int wins = 0;
                int losses = 0;
                int draws = 0;

                var points = 0;
                var goalsScored = 0;
                var goalsConceded = 0;

                foreach (var match in matchesForClub) //count points, goals, wins, losses, draws
                {
                    bool isHomeTeam = match.HomeClub.Id == club.Id;

                    if (isHomeTeam)
                    {
                        goalsScored += match.HomeGoals;
                        goalsConceded += match.AwayGoals;

                        if (match.HomeGoals > match.AwayGoals)
                        {
                            points += 3;
                            wins++;
                        }
                        else if (match.HomeGoals == match.AwayGoals)
                        {
                            points += 1;
                            draws++;
                        }
                        else
                        {
                            losses++;
                        }
                    }
                    else
                    { 
                        goalsScored += match.AwayGoals;
                        goalsConceded += match.HomeGoals;

                        if (match.AwayGoals > match.HomeGoals)
                        {
                            points += 3;
                            wins++;
                        }
                        else if (match.AwayGoals == match.HomeGoals)
                        {
                            points += 1;
                            draws++;
                        }
                        else
                        {
                            losses++;
                        }
                    }
                }

                clubStats.Add(new ClubStatViewModel
                {
                    ClubName = club.Name,
                    Points = points,
                    GoalsScored = goalsScored,
                    GoalsConceded = goalsConceded,
                    Wins = wins,
                    Losses = losses,
                    Draws = draws,
                    ClubSymbol = club.ImageFileId
                });
            }
            var model = new StatViewModel
            {
                MatchResults = matches,
                ClubStats = clubStats
            };
            return View(model);

        }


        // GET: StatsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        // GET: StatsController/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: StatsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        // GET: StatsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }
        // POST: StatsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        // GET: StatsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
        // POST: StatsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}