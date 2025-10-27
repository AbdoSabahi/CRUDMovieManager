using System.ComponentModel.DataAnnotations;

namespace APPMovies.Models
{
    public class Movie
    {

        public int Id { get; set; }
        


        [Required, MaxLength(250)]
        public string Titel { get; set; }

        public int Year { get; set; }


        public double Rate { get; set; }


        [Required, MaxLength(2500)]
        public string Storline { get; set; }




        public String Poster { get; set; }



        public int GenreId { get; set; }

        public Genre Genre { get; set; }
    }
}
