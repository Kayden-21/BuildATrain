using BuildATrain.Database.Models;
using Microsoft.EntityFrameworkCore;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {

    }

    //DbSets for models
    public DbSet<Attributes> Attributes { get; set; }
}
