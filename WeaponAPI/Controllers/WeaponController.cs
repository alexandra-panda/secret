using System;
using System.Threading.Tasks;
using Common.Models;
using Microsoft.AspNetCore.Mvc;
using WeaponAPI.Repositories.Abstracts;
using WeaponAPI.Services.Abstracts;

namespace WeaponAPI.Controllers
{
    [Route("weapon")]
    [ApiController]
    public class WeaponController : ControllerBase
    {
        private readonly IWeaponRepository _weaponRepository;
        private readonly ISyncService<Weapon> _syncService;

        public WeaponController(IWeaponRepository weaponRepository,
            ISyncService<Weapon> syncService)
        {
            _weaponRepository = weaponRepository;
            _syncService = syncService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _weaponRepository.GetAll();
            return Ok(result);
        }
        
        [HttpGet("{weaponId}")]
        public async Task<IActionResult> GetById(Guid weaponId)
        {
            var result = await _weaponRepository.GetById(weaponId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Weapon weapon)
        {
            var result = await _weaponRepository.Create(weapon);
            _syncService.Upsert(result);
            return Ok(result);
        }
        
        [HttpPut]
        public async Task<IActionResult> Upsert(Weapon weapon)
        {
            var result = await _weaponRepository.Upsert(weapon);
            _syncService.Upsert(result);
            return Ok(result);
        }

        [HttpDelete("{weaponId}")]
        public async Task<IActionResult> DeleteById(Guid weaponId)
        {
            var weapon = await _weaponRepository.GetById(weaponId);
            
            if (weapon == null)
                return BadRequest("Weapon not found");
            
            await _weaponRepository.DeleteById(weaponId);
            
            weapon.UpdatedTimestamp = DateTime.UtcNow;
            _syncService.Delete(weapon);
            
            return Ok($"Deleted: {weaponId}");
        }

        [HttpPut("sync")]
        public async Task<IActionResult> UpsertSync(Weapon weapon)
        {
            var existingWeapon = await _weaponRepository.GetById(weapon.Id);
            if (existingWeapon == null || existingWeapon.UpdatedTimestamp < weapon.UpdatedTimestamp)
            {
                await _weaponRepository.Upsert(weapon);
            }

            return Ok();
        }

        [HttpDelete("sync")]
        public async Task<IActionResult> DeleteSync(Weapon weapon)
        {
            var existingWeapon = await _weaponRepository.GetById(weapon.Id);

            if (existingWeapon != null && existingWeapon.UpdatedTimestamp < weapon.UpdatedTimestamp)
            {
                await _weaponRepository.DeleteById(weapon.Id);
            }

            return Ok();
        }
    }
}