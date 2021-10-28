using System.Threading.Tasks;
using Common.Models;
using Microsoft.AspNetCore.Mvc;
using SyncNode.Services;

namespace SyncNode.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SyncController : ControllerBase
    {
        private readonly SyncWorkJobService _syncWorkJobService;

        public SyncController(SyncWorkJobService syncWorkJobService)
        {
            _syncWorkJobService = syncWorkJobService;
        }
        
        [HttpPost]
        public async Task<IActionResult> Sync(SyncEntity entity)
        {
            _syncWorkJobService.AddItem(entity);
            return Ok();
        }
    }
}