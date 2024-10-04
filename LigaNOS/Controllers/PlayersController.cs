using LigaNOS.Data.Entities;
using LigaNOS.Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LigaNOS.Controllers
{
    public class PlayersController : Controller
    {

        private readonly IPlayerRepository _playerRepository;

        public PlayersController(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }
        // GET: PlayersController
        public async Task<IActionResult> Index()
        {
            var players = await _playerRepository.GetAllPlayersAsync();
            return View(players);
        }
        // GET: PlayersController/Details/5
        public async Task<ActionResult>Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _playerRepository.GetPlayerByIdAsync(id.Value);
            if (player == null)
                return NotFound();

            return View(player);
        }

        // GET: PlayersController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PlayersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Player player)
        {
            if (ModelState.IsValid)
            {
                await _playerRepository.CreatePlayerAsync(player);
                return RedirectToAction(nameof(Index));
            }
            return View(player);
        }

        // GET: PlayersController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var player = await _playerRepository.GetPlayerByIdAsync(id);
            if (player == null)
                return NotFound();

            return View(player);
        }

        // POST: PlayersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Player player)
        {
            if (ModelState.IsValid)
            {
                await _playerRepository.UpdatePlayerAsync(player);
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


            var player = await _playerRepository.GetPlayerByIdAsync(id.Value);
            if (player == null)
                return NotFound();

            return View(player);
        }

        // POST: PlayersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
         public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _playerRepository.DeletePlayerAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
