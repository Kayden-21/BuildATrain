using BuildATrain.Database.Models;
using BuildATrain.Models.Game;
using Microsoft.EntityFrameworkCore;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {

    }

    //DbSets for models
    public DbSet<Attributes> Attributes { get; set; }
    public DbSet<TrainModel> TrainModels { get; set; }
    public DbSet<WalletModel> WalletModels { get; set; }

}
