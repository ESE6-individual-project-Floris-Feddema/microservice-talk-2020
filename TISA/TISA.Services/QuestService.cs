using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Flurl.Http;
using TISA.Models;

namespace TISA.Services
{
    internal class QuestService : IQuestService
    {
        private readonly IPlayerService _playerService;

        public QuestService(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        public async Task<IEnumerable<Quest>> GetAvailableQuestsForPlayerAsync()
        {
            return await $"http://localhost:7500/PlayerQuest/{_playerService.Player.Id}".GetJsonAsync<IEnumerable<Quest>>();
        }

        public async Task<Quest> GetQuestByIdAsync(Guid questId)
        {
            return await $"http://localhost:7500/Quest/{questId}".GetJsonAsync<Quest>();
        }

        public async Task<IEnumerable<Quest>> GetAllQuestsAsync()
        {
            return await $"http://localhost:7500/Quest/".GetJsonAsync<IEnumerable<Quest>>();
        }

        public Task DeletByIdAsync(Guid questId)
        {
            return $"http://localhost:7500/Quest/{questId}".DeleteAsync();
        }

        public Task CreateAsync(Quest quest)
        {
            return $"http://localhost:7500/Quest/".PostJsonAsync(quest);
        }

        public Task UpdateQuestByIdAsync(Guid questId, Quest quest)
        {
            return $"http://localhost:7500/Quest/{questId}".PutJsonAsync(quest);
        }

        public Task CompleteQuestAsync(Guid questId)
        {
            return $"http://localhost:7500/PlayerQuest/{_playerService.Player.Id}".PostJsonAsync(questId.ToString());
        }
    }
}
