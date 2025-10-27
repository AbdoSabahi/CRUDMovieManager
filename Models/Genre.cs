using System.ComponentModel.DataAnnotations;

namespace APPMovies.Models
{
    public class Genre
    {
        public int Id { get; set; }


        [Required, MaxLength(100)]
        public string Name { get; set; }

        public IEnumerable<Movie> Movies { get; set; } = new List<Movie>();
    }
}
