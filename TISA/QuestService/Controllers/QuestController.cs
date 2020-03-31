using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestService.Database;

namespace QuestService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class QuestController : ControllerBase
    {

        private readonly QuestDbContext _dbContext;

        public QuestController(QuestDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Quest>>> GetQuests()
        {
            return await _dbContext.Quests.ToArrayAsync();
        }

        [HttpPost]
        public async Task<IActionResult> CreateQuest(Quest quest)
        {
            _dbContext.Quests.Add(quest);
            await _dbContext.SaveChangesAsync();
            return Created($"/Quest/{quest.Id}", quest);
        }

        [HttpGet("{questId}")]
        public async Task<ActionResult<Quest>> GetQuestById(Guid questId)
        {
            var q = await _dbContext.Quests.FirstOrDefaultAsync(qu => qu.Id == questId);

            if (q == null)
            {
                return NotFound();
            }

            return q;

        }

        [HttpDelete("{questId}")]
        public async Task<IActionResult> DeleteQuest(Guid questId)
        {
            var q = await _dbContext.Quests.FirstOrDefaultAsync(qu => qu.Id == questId);

            if (q == null)
            {
                return NotFound();
            }

            _dbContext.Quests.Remove(q);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("{questId}")]
        public async Task<IActionResult> UpdateQuest(Guid questId, Quest quest)
        {
            quest.Id = questId;
            _dbContext.Attach(quest).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
