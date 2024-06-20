using JokesWebApp.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace JokesWebApp.Data
{
    public class JokesDbContext : DbContext
    {
        public JokesDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet <Joke> Jokes{ get; set; }
    }
}
