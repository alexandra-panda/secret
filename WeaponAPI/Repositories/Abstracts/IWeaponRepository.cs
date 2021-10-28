using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Models;

namespace WeaponAPI.Repositories.Abstracts
{
    public interface IWeaponRepository
    {
        Task<Weapon> GetById(Guid weaponId);
        Task<IEnumerable<Weapon>> GetAll();
        Task<Weapon> Create(Weapon weapon);
        Task<Weapon> Upsert(Weapon weapon);
        Task DeleteById(Guid weaponId);
    }
}