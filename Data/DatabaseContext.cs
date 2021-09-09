using AddressAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AddressAPI.Data 
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
        }

        public DbSet<Address> Addresses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options){
            options.UseSqlite($"Filename=database.db");
        }
    }
}
