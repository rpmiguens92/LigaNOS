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
            return View(_clubRepository.GetAllWithClubs().OrderBy(c => c.Name));
        }

        // GET: ClubsController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var club = await _clubRepository.GetByIdAsync(id.Value);

            if (club == null)
            {
                return NotFound();
            }

            return View(club);
        }

        // GET: ClubsController/Create
        public IActionResult Create()
        {
            return View();

        }

        // POST: ClubsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClubViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "clubs");

                }
                var club = _converterHelper.ToClub(model, imageId, true);

                club.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                await _clubRepository.UpdateAsync(club);

                await _clubRepository.CreateAsync(club);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: ClubsController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var club = await _clubRepository.GetByIdAsync(id.Value);
            if (club == null)
            {
                return NotFound();
            }
            var model = _converterHelper.ToClubViewModel(club);
            return View(model);

        }

        // POST: ClubsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ClubViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Guid imageId = Guid.Empty;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {

                        imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "clubs");
                    }
                    var club = _converterHelper.ToClub(model, imageId, false);

                    club.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _clubRepository.ExistAsync(model.Id))
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

            // GET: ClubsController/Delete/5
            public async Task<IActionResult> Delete(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var club = await _clubRepository.GetByIdAsync(id.Value);

                if (club == null)
                {
                    return NotFound();
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
                    return NotFound();
                }

                await _clubRepository.DeleteAsync(club);
                return RedirectToAction(nameof(Index));
            }
        }
    }


