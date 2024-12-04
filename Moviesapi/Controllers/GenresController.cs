using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moviesapi.Models;
using Microsoft.EntityFrameworkCore;
using Moviesapi.Dtos;

namespace Moviesapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GenresController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var genres=await _context.Genres.ToListAsync();
            return Ok(genres);

        }
        //add data
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateGenreDto dto)
        {
         
            var genre=new Genre { Name = dto.name };
            await   _context.AddAsync(genre);
            _context.SaveChanges();
            return Ok(genre); //بعرض المحتوي
        }
        //update
        [HttpPut("id")]
        public async Task<IActionResult>UpdateAsync(int id, [FromBody]CreateGenreDto dto) // هديله اي دي + احطله الاسم الي عاوزه يعدله في البادي
        {
            var genre = await _context.Genres.SingleOrDefaultAsync(g=>g.Id ==id);
            if (genre==null)
            {
                return NotFound($"No genre was found with id {id}"); //checking
            }
            genre.Name= dto.name;
            _context.SaveChanges();
            return Ok(genre);
        }
        [HttpDelete]

        public async Task<IActionResult>DeleteAysnc(int id)
        {

            var genre = await _context.Genres.SingleOrDefaultAsync(g => g.Id == id);
            if (genre == null)
            {
                return NotFound($"No genre was found with id {id}");
            }

            _context.Remove(genre);
            _context.SaveChanges();
            return Ok(genre);
        }


    }
}
