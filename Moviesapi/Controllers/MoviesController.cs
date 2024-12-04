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

        [HttpGet]
        public async Task<IActionResult> GetallCreateAsync()
        {
            var movies =  await _context.Movies.Include(g=>g.Genre).ToListAsync();
            return Ok(movies);

        }
        [HttpGet("id")] //بجبر اليوزر يدخل id + عشان اتجنب ايرور ان مينفعش يكون عندي اتنين اند بوينت بنفس الوظيفه
        public async Task<IActionResult> GetById(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
                return BadRequest("not found");

            return Ok(movie);
        }
        [HttpGet("getgenreid")]
        public async Task<IActionResult>GetByGenreId(byte genid)
        {
            var movies = await _context.Movies.Where(m=>m.GenreId==genid)
                .OrderByDescending(x=>x.rate).Include(g => g.Genre).ToListAsync();
            return Ok(movies);
        }
        [HttpPost]
        public async Task<IActionResult>CreateAsync([FromForm]Moviedtos dto)

        {
            if (dto.Poster == null)
                return BadRequest("poster is required!");
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
        [HttpPut("{id}")]
        public async Task<IActionResult> Updateasync(int id, [FromForm] Moviedtos dto)
        {
            
            if (dto.Poster.Length > _maxallowedpostersize)
                return BadRequest("you can't exceed the maximum size for as it is 1MB"); //بشيك لو حجم الصوره اكبر من الي انا عاوزه
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null) return NotFound($"no movie was found with {id}");
            var isvaildgenre=await _context.Genres.AnyAsync(g=>g.Id==dto.GenreId);
            if (!isvaildgenre)
                return BadRequest("invalid genre id");
            if (dto.Poster != null) {
                if (!_allowedextenstions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                    return BadRequest("only png and jpg are allowed!");

                if (dto.Poster.Length > _maxallowedpostersize)
                    return BadRequest("max allowed size is not 1 mb");
                using var datastream=new MemoryStream();
                await dto.Poster.CopyToAsync(datastream);
                movie.Poster=datastream.ToArray();
            }
            movie.Title = dto.Title;
            movie.Storyline = dto.Storyline;
            movie.rate = dto.rate;  
            movie.year = dto.year;
            movie.GenreId = dto.GenreId;

            _context.SaveChanges();
            return Ok(movie);   
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult>Deleteasync(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null) return NotFound($"no movie was found with that {id}");

            _context.Remove(movie);
            _context.SaveChanges();
            return Ok(movie);
        }
    }
  

}
