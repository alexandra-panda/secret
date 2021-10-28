using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Models;
using Microsoft.EntityFrameworkCore;
using WeaponAPI.DbContext;
using WeaponAPI.Repositories.Abstracts;

namespace WeaponAPI.Repositories
{
    public class WeaponRepository: IWeaponRepository
    {
        private readonly WeaponDbContext _context;
        
        public WeaponRepository(WeaponDbContext context)
        {
            _context = context;
        }
        
        public async Task<Weapon> GetById(Guid weaponId)
        {
            return await _context.Weapons
                .FirstOrDefaultAsync(w => w.Id == weaponId);
        }

        public async Task<IEnumerable<Weapon>> GetAll()
        {
            return await _context.Weapons
                .ToListAsync();
        }

        public async Task<Weapon> Create(Weapon weapon)
        {
            weapon.UpdatedTimestamp = DateTime.UtcNow;
            await _context.AddAsync(weapon);
            await _context.SaveChangesAsync();
            return weapon;
        }

        public async Task<Weapon> Upsert(Weapon weapon)
        {
            var existingWeapon = await GetById(weapon.Id);

            if (existingWeapon == null)
                return await Create(weapon);
            else
                _context.Entry(existingWeapon).State = EntityState.Detached;

            weapon.UpdatedTimestamp = DateTime.UtcNow;
            _context.Entry(weapon).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return weapon;
        }

        public async Task DeleteById(Guid weaponId)
        {
            var weapon = await GetById(weaponId);
            if (weapon != null)
            {
                _context.Weapons.Remove(weapon);
                await _context.SaveChangesAsync();
            }
        }
    }
}