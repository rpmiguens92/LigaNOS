using LigaNOS.Data;
using LigaNOS.Data.Entities;
using LigaNOS.Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace LigaNOS.Controllers
{
    public class ClubsController : Controller
    {
        private readonly IClubRepository _clubRepository;
        public ClubsController(IClubRepository clubRepository)
        {
            _clubRepository = clubRepository;
        }

        // GET: ClubsController
        public IActionResult Index()
        {
            return View(_clubRepository.GetAll().OrderBy(c => c.Name));
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
        public async Task<IActionResult> Create(Club club)
        {
            if (ModelState.IsValid)
            {

                await _clubRepository.CreateAsync(club);
                return RedirectToAction(nameof(Index));
            }
            return View(club);
        }

        // GET: ClubsController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var club = await _clubRepository.GetByIdAsync(id.Value);
            if (club == null)
            {
                return NotFound();
            }
            return View(club);
        }

        // POST: ClubsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Club club)
        {
            if (id != club.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _clubRepository.UpdateAsync(club);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _clubRepository.ExistAsync(club.Id))
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
                return View(club);
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


