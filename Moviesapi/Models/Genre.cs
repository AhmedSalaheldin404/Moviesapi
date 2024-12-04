
using System.ComponentModel.DataAnnotations.Schema;

namespace Moviesapi.Models
{
    public class Genre
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte Id { get; set; }
        [MaxLength(length:100)]
        public string Name { get; set; }
    }
}
