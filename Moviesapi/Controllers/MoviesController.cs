using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moviesapi.Dtos;
using Moviesapi.Models;

namespace Moviesapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    
    {
        private readonly ApplicationDbContext _context;
        private new List<string> _allowedextenstions= new List<string>() { ".jpg,",".png"};
        private long _maxallowedpostersize = 1048576;

        public MoviesController(ApplicationDbContext context)

        {
            _context = context;
        }
        //add movie

        [HttpPost]
        public async Task<IActionResult>CreateAsync([FromForm]Moviedtos dto)

        {
            if (!_allowedextenstions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                return BadRequest("only png and jpg are allowed!"); // بشيك ان المسار الي في البوستر مش بيحتوي علي jpg or png
            if (dto.Poster.Length > _maxallowedpostersize)
                return BadRequest("you can't exceed the maximum size for as it is 1MB"); //بشيك لو حجم الصوره اكبر من الي انا عاوزه
            var isvaildgenre= await _context.Genres.AnyAsync(g=>g.Id==dto.GenreId); // بشيك لو اليوزردخل id for genre مش معانا فا هيظهر الايرور دا
            if (!isvaildgenre)
                return BadRequest("invalid genre id");
            using var datastream=new MemoryStream(); //تخزين الداتا مؤقتا واستخدمت using عشان امسح الداتا بعد ما اخلص
            await dto.Poster.CopyToAsync(datastream); //انسخ محتوي بوستر (الصوره) واحطها في داتا ستريم
            var movie = new Movie
            {
                GenreId = dto.GenreId,
                Title = dto.Title,
                Poster = datastream.ToArray(), // حولته اراي عشان اقدر اخزنه في الداتا بيزٍ
                rate = dto.rate,
                Storyline = dto.Storyline,
                year = dto.year
            };
            await _context.AddAsync(movie);
            _context.SaveChanges();
            return Ok(movie);
        }

    }
}
