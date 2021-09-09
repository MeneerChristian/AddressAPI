
using Microsoft.EntityFrameworkCore;

namespace AddressAPI.Data 
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options){
            options.UseSqlite($"Filename=database.db");
        }
    }
}
