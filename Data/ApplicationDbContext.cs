using APPMovies.Models;
using System.Collections.Generic;
using System.Reflection.Emit;


namespace APPMovies.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
         : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Genre>()
                .HasMany(g => g.Movies)
                .WithOne(m => m.Genre)
                .HasForeignKey(m => m.GenreId);


            modelBuilder.Entity<Genre>().HasData(
              new Genre { Id = 1, Name = "Action" },
              new Genre { Id = 2, Name = "Comedy" },
              new Genre { Id = 3, Name = "Drama" },
              new Genre { Id = 4, Name = "Horror" },
              new Genre { Id = 5, Name = "Sci-Fi" }
          );

        }
      
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }

    }
}
