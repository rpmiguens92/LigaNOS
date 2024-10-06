using LigaNOS.Data;
using LigaNOS.Data.Entities;
using LigaNOS.Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LigaNOS.Controllers
{
    public class MatchesController : Controller
    {
        private readonly IMatchRepository _matchRepository;

        public MatchesController(IMatchRepository matchRepository)
        {
            _matchRepository = matchRepository;
        }

            // GET: MatchesController
            public IActionResult Index()
            {
           return View(_matchRepository.GetAll().OrderBy(m => m.MatchDay));

             }

        // GET: MatchesController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var match = await _matchRepository.GetByIdAsync(id.Value);
             
            if (match == null)
            {
                return NotFound();
            }

            return View(match);
        }

        // GET: MatchesController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MatchesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Match match)
        {
            if (ModelState.IsValid)
            { 
                await _matchRepository.CreateAsync(match);
                return RedirectToAction(nameof(Index));
            }
            return View(match);
        }

        // GET: MatchesController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var match = await _matchRepository.GetByIdAsync(id.Value);
            if (match == null)
            {
                return NotFound();
            }
            return View();
        }

        // POST: MatchesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit (int id,Match match)
        {
            if (id != match.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _matchRepository.UpdateAsync(match);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _matchRepository.ExistAsync(match.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(match);
        }

        // GET: MatchesController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var match = await _matchRepository.GetByIdAsync(id.Value);

            if (match == null)
            {
                return NotFound();
            }
            return View(match);
        }

        // POST: MatchesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var match = await _matchRepository.GetByIdAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
