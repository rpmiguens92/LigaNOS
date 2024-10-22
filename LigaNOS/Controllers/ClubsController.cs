using LigaNOS.Data;
using LigaNOS.Data.Entities;
using LigaNOS.Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using LigaNOS.Helpers;
using LigaNOS.Models;
using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;

namespace LigaNOS.Controllers
{

    public class ClubsController : Controller
    {
        private readonly IClubRepository _clubRepository;
        private readonly IUserHelper _userHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IBlobHelper _blobHelper;
        public ClubsController(IClubRepository clubRepository, IUserHelper userHelper, IConverterHelper converterHelper, IBlobHelper blobHelper)
        {
            _clubRepository = clubRepository;
            _userHelper = userHelper;
            _converterHelper = converterHelper;
            _blobHelper = blobHelper;
        }

        // GET: ClubsController

        public IActionResult Index()
        {
            return View(_clubRepository.GetAll().OrderBy(c => c.Name));
        }

        // GET: ClubsController/Details/5
        [Authorize(Roles = "Admin, Club")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewModel("ClubNotFound");
            }

            // var club = await _clubRepository.GetByIdAsync(id.Value);
            var club = await _clubRepository.GetAll()
                    .Include(c => c.Players) // Incluir jogadores
                    .FirstOrDefaultAsync(c => c.Id == id.Value);


            if (club == null)
            {
                return new NotFoundViewModel("ClubNotFound");
            }

            return View(club);
        }

        // GET: ClubsController/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();

        }

        // POST: ClubsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClubViewModel model)
        {
            var existingClub = await _clubRepository.GetAll()
           .FirstOrDefaultAsync(c => c.Name.ToLower() == model.Name.ToLower());

            if (existingClub != null)
            {
                ModelState.AddModelError(string.Empty, "Club already registed.");
                return View(model);
            }
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "clubs");

                }
                var club = _converterHelper.ToClub(model, imageId, true);

                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "Account");
                }

                club.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);


                await _clubRepository.CreateAsync(club);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: ClubsController/Edit/5
        [Authorize(Roles = "Admin ")]
        public async Task<IActionResult> Edit(int id)
        {

            var club = await _clubRepository.GetByIdAsync(id);
            if (club == null)
            {
                return new NotFoundViewModel("ClubNotFound");
            }
            var model = _converterHelper.ToClubViewModel(club);
            return View(model);

        }

        // POST: ClubsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ClubViewModel model)
        {
            if (id != model.Id)
            {
                return new NotFoundViewModel("ClubNotFound");
            }

            if (ModelState.IsValid)
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "Account");
                }

                try
                {
                    var club = await _clubRepository.GetByIdAsync(model.Id);
                    if (club == null)
                    {
                        return new NotFoundViewModel("ClubNotFound");
                    }
                    //check image
                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        club.ImageFileId = await _blobHelper.UploadBlobAsync(model.ImageFile, "clubs");
                    }


                    club.Name = model.Name;
                    club.Coach = model.Coach;
                    club.Stadium = model.Stadium;

                    club.User = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

                    await _clubRepository.UpdateAsync(club);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _clubRepository.ExistAsync(model.Id))
                    {
                        return new NotFoundViewModel("ClubNotFound");
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

        // GET: ClubsController/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewModel("ClubNotFound");
            }

            var club = await _clubRepository.GetByIdAsync(id.Value);

            if (club == null)
            {
                return new NotFoundViewModel("ClubNotFound");
            }

            return View(club);
        }

        // POST: ClubsController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var club = await _clubRepository.GetByIdAsync(id);
            if (club == null)
            {
                return new NotFoundViewModel("ClubNotFound");
            }

            var hasMatches = await _clubRepository.HasMatchesAsync(id);

            if (hasMatches)
            {
                ModelState.AddModelError(string.Empty, "Cannot delete this club because there are matches associated.");
                return View(club);
            }

            await _clubRepository.DeleteAsync(club);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult ClubNotFound()
        {
            return View();
        }
    }
}