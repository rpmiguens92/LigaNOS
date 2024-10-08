using System.Linq;
using System.Threading.Tasks;
using LigaNOS.Data.Entities;
using LigaNOS.Data.Repositories;
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

        public PlayersController(IPlayerRepository playerRepository, IClubRepository clubRepository)
        {
            _playerRepository = playerRepository;
            _clubRepository = clubRepository;
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
        public async Task<IActionResult> Create(Player player)
        {
            if (ModelState.IsValid)
            {
                await _playerRepository.CreateAsync(player);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Clubs = new SelectList(_clubRepository.GetAll(),"Id", "Name", player.ClubId);
            return View(player);
        }

        // GET: PlayersController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var player = await _playerRepository.GetByIdAsync(id.Value);
            if (player == null)
                return NotFound();

            return View(player);
        }

        // POST: PlayersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,Player player)
        {
            if(id != player.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    await _playerRepository.UpdateAsync(player);
                }
                catch (DbUpdateConcurrencyException) 
                {
                    if (!await _playerRepository.ExistAsync(player.Id))
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
            return View(player);
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
