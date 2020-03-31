using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlayerService.Database;

namespace PlayerService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayerController : ControllerBase
    {

        private readonly PlayerDbContext _context;

        public PlayerController(PlayerDbContext context)
        {
            _context = context;
        }

        [HttpGet("{playerId}")]
        public async Task<ActionResult<Player>> GetPlayer(Guid playerId)
        {
            var p =  await _context.Players.FirstOrDefaultAsync(player => player.Id == playerId);
            if (p == null)
            {
                return NotFound();
            }
            return p;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewPlayer([FromBody] string playerName)
        {
            var p = new Player
            {
                Name = playerName,
                Experience = 1000,
                Gold = 10000,
                Level = 10,
            };

            _context.Players.Add(p);
            await _context.SaveChangesAsync();
            return Created($"/Player/{p.Id}", p);
        }
    }
}
