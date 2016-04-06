using SportStore.Domain.Entities;
using System.Data.Entity;


namespace SportStore.Domain.Concrete
{
    public class EFDbContext : DbContext
    {
        public EFDbContext(): base("EFDbContext")
        { }


        public DbSet<Game> Games { get; set; }
    }
}
