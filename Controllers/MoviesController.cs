using APPMovies.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace APPMovies.Controllers
{
    public class MoviesController : Controller
    {

        private readonly ApplicationDbContext _Context;

        public MoviesController(ApplicationDbContext context)
        {
            _Context = context;
        }
        public async Task<IActionResult> Index()
        {
            var movies = await _Context.Movies.ToListAsync();
            return View(movies);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var genresList = await _Context.Genres
                .OrderBy(g => g.Name)
                .Select(g => new SelectListItem
                {
                    Value = g.Id.ToString(),
                    Text = g.Name
                })
                .ToListAsync();
            var viewModels = new MovieFormViewModel()
            {
                Genres = genresList,
            };

            return View(viewModels);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MovieFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Genres = await _Context.Genres
                    .OrderBy(g => g.Name)
                    .Select(g => new SelectListItem
                    {
                        Value = g.Id.ToString(),
                        Text = g.Name
                    })
                    .ToListAsync();

                return View("Create", model);
            }

            Movie movie;

            if (model.Id == 0)
            {
                // إنشاء كائن جديد من Movie
                movie = new Movie
                {
                    Titel = model.Title,
                    Year = model.Year,
                    Rate = model.Rate,
                    Storline = model.Storyline,
                    GenreId = model.GenreId
                };

                // إضافة الفيلم للسياق
                _Context.Movies.Add(movie);
            }
            else
            {
                // تعديل فيلم موجود
                movie = await _Context.Movies.FindAsync(model.Id);
                if (movie == null) return NotFound();

                movie.Titel = model.Title;
                movie.Year = model.Year;
                movie.Rate = model.Rate;
                movie.Storline = model.Storyline;
                movie.GenreId = model.GenreId;
            }

            
            if (model.Poster != null)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(model.Poster.FileName);
                var postersPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "posters");

                if (!Directory.Exists(postersPath))
                    Directory.CreateDirectory(postersPath);

                var filePath = Path.Combine(postersPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Poster.CopyToAsync(stream);
                }

                if (!string.IsNullOrEmpty(movie.Poster))
                {
                    var oldPosterPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", movie.Poster.TrimStart('/'));
                    if (System.IO.File.Exists(oldPosterPath))
                        System.IO.File.Delete(oldPosterPath);
                }


                movie.Poster = fileName;
            }

            await _Context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    




    [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var movie = await _Context.Movies.FindAsync(id);

            if (movie == null)
                return NotFound();

            var viewModel = new MovieFormViewModel
            {
                Id = movie.Id,
                Title = movie.Titel,
                Year = movie.Year,
                Rate = movie.Rate,
                Storyline = movie.Storline,
                GenreId = movie.GenreId,
                Genres = await _Context.Genres
                    .OrderBy(g => g.Name)
                    .Select(g => new SelectListItem
                    {
                        Value = g.Id.ToString(),
                        Text = g.Name
                    })
                    .ToListAsync()
            };

            return View("Create", viewModel); // استخدم نفس صفحة الـ Create
        }


        public async Task<IActionResult> Delete(int id)
        {
            var movie = await _Context.Movies.FindAsync(id);

            if (movie == null)
                return NotFound();
            

            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/posters", movie.Poster);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
            _Context.Movies.Remove(movie);
            await _Context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var movie = await _Context.Movies
       .Include(m => m.Genre)
       .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
                return NotFound();

            return View(movie);
        }


    }







}