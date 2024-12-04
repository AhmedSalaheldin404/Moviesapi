namespace Moviesapi.Dtos
{
    public class Moviedtos
    {
        public string Title { get; set; }
        public int year { get; set; }
        public double rate { get; set; }
        [MaxLength(2500)]
        public string Storyline { get; set; }
        public IFormFile Poster { get; set; } //صوره
        public byte GenreId { get; set; }
    }
}
