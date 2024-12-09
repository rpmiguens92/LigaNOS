using LigaNOS.Data.Repositories;
using LigaNOS.Data.Entities;
using LigaNOS.Helpers;
using LigaNOS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using LigaNOS.Data.DTO;

namespace LigaNOS.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClubsController : ControllerBase
    {
        private readonly IClubRepository _clubRepository;
        private readonly IUserHelper _userHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IBlobHelper _blobHelper;

        public ClubsController(
            IClubRepository clubRepository,
            IUserHelper userHelper,
            IConverterHelper converterHelper,
            IBlobHelper blobHelper)
        {
            _clubRepository = clubRepository;
            _userHelper = userHelper;
            _converterHelper = converterHelper;
            _blobHelper = blobHelper;
        }

        // GET: api/clubs
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var clubs = await _clubRepository.GetAllAsync();
            var result = clubs.Select(c => new ClubDto
            {
                Id = c.Id,
                Name = c.Name,
                Coach = c.Coach,
                Stadium = c.Stadium,
                Wins = c.Wins,
                Losses = c.Losses,
                Draws = c.Draws,
                ImageUrl = c.ImageFullPath,
          
            });

            return Ok(result);
        }

        // GET: api/clubs/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var club = await _clubRepository.GetByIdAsync(id);

            if (club == null)
            {
                return NotFound(new { message = "Club not found." });
            }

            var clubDto = new ClubDto
            {
                Id = club.Id,
                Name = club.Name,
                Coach = club.Coach,
                Stadium = club.Stadium,
                Wins = club.Wins,
                Losses = club.Losses,
                Draws = club.Draws,
                ImageUrl = club.ImageFullPath, 
            };

            return Ok(clubDto);
        }

        // POST: api/clubs
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateClubDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Verifica se o clube já existe
            var clubs = await _clubRepository.GetAllAsync(); // Obtém todos os clubes
            var existingClub = clubs.FirstOrDefault(c => c.Name.ToLower() == model.Name.ToLower());

            if (existingClub != null)
            {
                return BadRequest(new { message = "Club already exists." });
            }

            // Upload da imagem (se fornecida)
            Guid imageId = Guid.Empty;
            if (model.ImageFile != null)
            {
                imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "clubs");
            }

            // Mapeia manualmente o DTO para a entidade Club
            var club = new Club
            {
                Name = model.Name,
                Coach = model.Coach,
                Stadium = model.Stadium,
                ImageFileId = imageId
            };

            // Associa o utilizador autenticado ao clube
            club.User = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

            // Cria o clube
            await _clubRepository.CreateAsync(club);

            // Retorna o clube criado
            return CreatedAtAction(nameof(GetById), new { id = club.Id }, new ClubDto
            {
                Id = club.Id,
                Name = club.Name,
                Coach = club.Coach,
                Stadium = club.Stadium,
                Wins = club.Wins,
                Losses = club.Losses,
                Draws = club.Draws,
                ImageUrl = club.ImageFullPath,
            });
        }

        // PUT: api/clubs/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateClubDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var club = await _clubRepository.GetByIdAsync(id);
            if (club == null)
            {
                return NotFound(new { message = "Club not found." });
            }

            if (model.ImageFile != null)
            {
                club.ImageFileId = await _blobHelper.UploadBlobAsync(model.ImageFile, "clubs");
            }

            club.Name = model.Name;
            club.Coach = model.Coach;
            club.Stadium = model.Stadium;

            await _clubRepository.UpdateAsync(club);

            return Ok(new { message = "Club updated successfully." });
        }

        // DELETE: api/clubs/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var club = await _clubRepository.GetByIdAsync(id);
            if (club == null)
            {
                return NotFound(new { message = "Club not found." });
            }

            var hasMatches = await _clubRepository.HasMatchesAsync(id);
            if (hasMatches)
            {
                return BadRequest(new { message = "Cannot delete club with associated matches." });
            }

            await _clubRepository.DeleteAsync(club);
            return Ok(new { message = "Club deleted successfully." });
        }
    }
}