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
        public PlayersController(IPlayerRepository playerRepository, IClubRepository clubRepository, IUserHelper userHelper, IConverterHelper converterHelper, IBlobHelper blobHelper)
        {
            _playerRepository = playerRepository;
            _clubRepository = clubRepository;
            _userHelper = userHelper;
            _converterHelper = converterHelper;
            _blobHelper = blobHelper;
             
        }
        // GET: PlayersController
        public IActionResult Index()
        {
            var players = _playerRepository.GetAll().Include(p => p.Clubs).OrderBy(p => p.Name).ToList();
 
            return View(players);
        }
            // GET: PlayersController/Details/5
            public async Task<IActionResult> Details(int? id)
            {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _playerRepository.GetByIdAsync(id.Value);
            if (player == null)
            {
                return NotFound();
            }

            return View(player);
            }

        // GET: PlayersController/Create
        public IActionResult Create()
        {
            var clubs = _clubRepository.GetAll().ToList();
            clubs.Insert(0, new Club { Id = 0, Name = "-- No Club Selected --" });

           
            ViewBag.Clubs = new SelectList(clubs, "Id", "Name");
            return View();
        }

        // POST: PlayersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PlayerViewModel model)
        {
            var existingPlayer = await _playerRepository.GetAll()
          .FirstOrDefaultAsync(c => c.Name.ToLower() == model.Name.ToLower());

            if (existingPlayer != null)
            {
                ModelState.AddModelError(string.Empty, "Player already registed.");
                return View(model);
            }


            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "players");
                }
                var player = _converterHelper.ToPlayer(model, imageId, true);

                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "Account");
                }

                player.User = await _userHelper.GetUserByEmailAsync(User.Identity.Name);


                //teste
                if (model.ClubId != 0)  
                {
                    player.ClubId = model.ClubId;
                }
                else
                {
                    player.ClubId = null;  
                }


                await _playerRepository.CreateAsync(player);

                return RedirectToAction(nameof(Index));
            }
            var clubs = _clubRepository.GetAll().ToList();
            clubs.Insert(0, new Club { Id = 0, Name = "-- No Club Selected --" });
            ViewBag.Clubs = new SelectList(clubs, "Id", "Name", model.ClubId);

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
            //teste
            var clubs = _clubRepository.GetAll().ToList();
            clubs.Insert(0, new Club { Id = 0, Name = "-- No Club Selected --" });

            ViewBag.Clubs = new SelectList(clubs, "Id", "Name", model.ClubId);

            return View(model);
        }

        // POST: PlayersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PlayerViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "Account");
                }
                try
                {
                    var player = await _playerRepository.GetByIdAsync(model.Id);
                    if (player == null) 
                    { 
                        return NotFound(); 
                    }


                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {

                        player.ImageFileId = await _blobHelper.UploadBlobAsync(model.ImageFile, "players");
                       
                    }


                    player.Name = model.Name;
                    player.DateOfBirth = model.DateOfBirth;
                    player.Position = model.Position;

                    //teste

                    if (model.ClubId == 0)
                    {
                        player.ClubId = null;
                    }
                    else
                    {
                        player.ClubId = model.ClubId;
                    }
                    player.User = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

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
            var clubs = _clubRepository.GetAll().ToList();
            clubs.Insert(0, new Club { Id = 0, Name = "-- No Club Selected --" });
            ViewBag.Clubs = new SelectList(clubs, "Id", "Name", model.ClubId);

 
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
            {
                return NotFound();
            }

            return View(player);
        }

        // POST: PlayersController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var player = await _playerRepository.GetByIdAsync(id);
            if (player == null)
            {
                return NotFound();
            }
            await _playerRepository.DeleteAsync(player);
            return RedirectToAction(nameof(Index));
        }
    }
}