using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace APPMovies.ViewModels
{
    public class MovieFormViewModel
    {
        public int Id { get; set; }

        [Required, StringLength(250)]
        [Display(Name = "Title")]
        public string Title { get; set; }  // ✅ تصحيح كلمة Titel → Title

        [Range(1900, 2100)]
        public int Year { get; set; }

        [Range(1, 10)]
        public double Rate { get; set; }

        [Required, StringLength(2500)]
        [Display(Name = "Storyline")]
        public string Storyline { get; set; }  // ✅ تصحيح Storline → Storyline

        [Display(Name = "Poster")]
        public IFormFile? Poster { get; set; }

        [Display(Name = "Genre")]
        [Required(ErrorMessage = "Please select a genre")]
        public int GenreId { get; set; }

        public IEnumerable<SelectListItem> Genres { get; set; } = new List<SelectListItem>();
    }
}
