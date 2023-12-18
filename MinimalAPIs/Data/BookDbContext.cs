using Microsoft.EntityFrameworkCore;
using MinimalAPIs.Model;

namespace MinimalAPIs.Data;

public class BookDbContext : DbContext
{
    public BookDbContext(DbContextOptions<BookDbContext> opts)
        : base(opts) { }

    public DbSet<Book> Books { get; set; }
}
