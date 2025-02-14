using Microsoft.EntityFrameworkCore;

namespace Reto.Db
{
    public class ApplicationContext : DbContext
    {

        public ApplicationContext()
        {
            SQLitePCL.Batteries_V2.Init();

            this.Database.EnsureCreated();
            this.Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "reto.db3");

            optionsBuilder
                .UseSqlite($"Filename={dbPath}");
            optionsBuilder.EnableSensitiveDataLogging(true);
        }

        public DbSet<Db.Entities.Taller> Taller { get; set; }
        public DbSet<Db.Entities.Piezas> Piezas { get; set; }
        public DbSet<Db.Entities.Refaccion> Refaccion { get; set; }
    }
}
