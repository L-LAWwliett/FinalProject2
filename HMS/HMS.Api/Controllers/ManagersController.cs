using HMS.Application.DTOs.Manager;
using HMS.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ManagersController : ControllerBase
    {
        private readonly IManagerService _managerService;

        public ManagersController(IManagerService managerService)
        {
            _managerService = managerService;
        }

        // GET: api/managers
        [HttpGet]
        public async Task<IActionResult> GetAllManagers()
        {
            var managers = await _managerService.GetAllManagersAsync();
            return Ok(managers);
        }

        // GET: api/managers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetManager(int id)
        {
            var manager = await _managerService.GetManagerByIdAsync(id);
            if (manager == null) return NotFound("მენეჯერი ვერ მოიძებნა.");

            return Ok(manager);
        }

        // POST: api/managers
        [HttpPost]
        public async Task<IActionResult> CreateManager([FromBody] CreateManagerDto createDto)
        {
            // აქ მოხდება უნიკალურობის (Email, PersonalNumber) და სასტუმროს შემოწმება
            var manager = await _managerService.CreateManagerAsync(createDto);
            return CreatedAtAction(nameof(GetManager), new { id = manager.Id }, manager);
        }

        // PUT: api/managers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateManager(int id, [FromBody] UpdateManagerDto updateDto)
        {
            await _managerService.UpdateManagerAsync(id, updateDto);
            return NoContent();
        }

        // DELETE: api/managers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteManager(int id)
        {
            // აქ მოწმდება, არის თუ არა სხვა მენეჯერიც ამ სასტუმროში წაშლამდე
            await _managerService.DeleteManagerAsync(id);
            return NoContent();
        }
    }
}