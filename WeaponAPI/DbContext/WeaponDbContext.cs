using Common.Models;
using Microsoft.EntityFrameworkCore;

namespace WeaponAPI.DbContext
{
    public class WeaponDbContext: Microsoft.EntityFrameworkCore.DbContext
    {
        public WeaponDbContext(DbContextOptions<WeaponDbContext> options) : base(options) 
        { }

        public DbSet<Weapon> Weapons { get; set; }
    }
}