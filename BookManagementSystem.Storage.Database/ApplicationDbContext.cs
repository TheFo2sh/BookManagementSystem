using BookManagementSystem.Storage.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookManagementSystem.Storage.Database
{
    public class ApplicationDbContext : DbContext
    {
        public virtual DbSet<BookEntity> Books { get; set; }
        public virtual DbSet<CategoryEntity> Categories { get; set; }
        public virtual DbSet<AuthorEntity> Authors { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }
    }
}