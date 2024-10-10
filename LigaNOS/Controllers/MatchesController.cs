using LigaNOS.Data;
using LigaNOS.Data.Entities;
using LigaNOS.Data.Repositories;
using LigaNOS.Helpers;
using LigaNOS.Models;
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
        private readonly IUserHelper _userHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly IConverterHelper _converterHelper;

        public MatchesController(IMatchRepository matchRepository, IUserHelper userHelper, IBlobHelper blobHelper, IConverterHelper converterHelper)
        {
            _matchRepository = matchRepository;
            _userHelper = userHelper;
            _blobHelper = blobHelper;
            _converterHelper = converterHelper;
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
        public async Task<IActionResult> Create(MatchViewModel model)
        {
            if (ModelState.IsValid)
            {

                Guid imageId = Guid.Empty;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "matches");
                }

                var match = _converterHelper.ToMatch(model, imageId, true);

                match.User = await _userHelper.GetUserByEmailAsync("miguens.rp@gmail.com");
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
                return NotFound();
            }

            var match = await _matchRepository.GetByIdAsync(id.Value);
            if (match == null)
            {
                return NotFound();
            }
            var model = _converterHelper.ToMatchViewModel(match);

            return View(model);
        }

        // POST: MatchesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit (MatchViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Guid imageId = Guid.Empty;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {

                        imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "matches");
                    }
                    var match = _converterHelper.ToMatch(model, imageId, false);

                    match.User = await _userHelper.GetUserByEmailAsync("miguens.rp@gmail.com");
                    await _matchRepository.UpdateAsync(match);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _matchRepository.ExistAsync(model.Id))
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
            return View(model);
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
