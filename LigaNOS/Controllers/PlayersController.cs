using System;
using System.Linq;
using System.Threading.Tasks;
using LigaNOS.Data;
using LigaNOS.Data.Entities;
using LigaNOS.Data.Repositories;
using LigaNOS.Helpers;
using LigaNOS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
 


namespace LigaNOS.Controllers
{
    public class PlayersController : Controller
    {

        private readonly IPlayerRepository _playerRepository;
        private readonly IClubRepository _clubRepository;
        private readonly IConverterHelper _converterHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly IUserHelper _userHelper;
        private readonly DataContext _context;
        public PlayersController(IPlayerRepository playerRepository, IClubRepository clubRepository, IUserHelper userHelper, IConverterHelper converterHelper, DataContext context)
        {
            _playerRepository = playerRepository;
            _clubRepository = clubRepository;
            _userHelper = userHelper;
            _converterHelper = converterHelper;
            _context = context;
        }
        // GET: PlayersController
        public IActionResult Index()
        {
            var players = _playerRepository.GetAll().Include(p => p.Clubs).OrderBy(p => p.Name).ToList();
            var clubs = _clubRepository.GetAll().OrderBy(c => c.Name).ToList();

            var viewModel = new PlayersAndClubsViewModel
            {
                Players = players,
                Clubs = clubs
            };

            return View(viewModel);
        }
        // GET: PlayersController/Details/5
        public async Task<IActionResult>Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _playerRepository.GetByIdAsync(id.Value);
            if (player == null)
                return NotFound();

            return View(player);
        }

        // GET: PlayersController/Create
        public IActionResult Create()
        {
            ViewBag.Clubs = new SelectList(_clubRepository.GetAll(),"Id", "Name");
            return View();
        }

        // POST: PlayersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PlayerViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "players");
                }
                var player = _converterHelper.ToPlayer(model, imageId, true);
                player.User = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                await _playerRepository.CreateAsync(player);

                return RedirectToAction(nameof(Index));
            }
            ViewBag.Clubs = new SelectList(_clubRepository.GetAll(),"Id", "Name", model.ClubId);
           
            return View(model);
        }

        // GET: PlayersController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var player = await _playerRepository.GetByIdAsync(id.Value);

            if (player == null)
            {
                return NotFound();
            }
            var model = _converterHelper.ToPlayerViewModel(player);
            return View(model);
        }

        // POST: PlayersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PlayerViewModel model)
        { 
            if (ModelState.IsValid)
            {
                try
                {
                    Guid imageId = Guid.Empty;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {

                        imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "players");

                    }
                    var player = _converterHelper.ToPlayer(model, imageId, false);
                    //player.User = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                    player.User = _context.Users.FirstOrDefault();
                    await _playerRepository.UpdateAsync(player);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _playerRepository.ExistAsync(model.Id))
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

        // GET: PlayersController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var player = await _playerRepository.GetByIdAsync(id.Value);
            if (player == null)
                return NotFound();

            return View(player);
        }

        // POST: PlayersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
         public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var player = await _playerRepository.GetByIdAsync(id);
            await _playerRepository.DeleteAsync(player);
            return RedirectToAction(nameof(Index));
        }
    }
}
