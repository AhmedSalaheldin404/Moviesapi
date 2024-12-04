namespace Moviesapi.Dtos
{
    public class CreateGenreDto
    {
        [MaxLength(100)]
        public string name { get; set; }
    }
}
