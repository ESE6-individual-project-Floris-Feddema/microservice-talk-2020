using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PlayerService.Database;
using Shared.Messaging;
using PlayerService.Messages;

namespace PlayerService.MessageHandlers 
{
    public class QuestCompletedMessageHandler : IMessageHandler<QuestCompleted>
    {
        private readonly PlayerDbContext _context;

        public QuestCompletedMessageHandler(PlayerDbContext context)
        {
            _context = context;
        }

        public async Task HandleMessageAsync(string messageType, QuestCompleted message)
        {
            var player = await _context.Players.FirstOrDefaultAsync(p => p.Id == message.PlayerId);
            if (player == null)
            {
                return;
            }

            player.Experience += message.Quest.ExperienceReward;
            player.Gold += message.Quest.GoldReward;

            await _context.SaveChangesAsync();
        }
    }
}