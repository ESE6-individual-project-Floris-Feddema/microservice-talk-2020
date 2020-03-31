using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Flurl.Http;
using TISA.Models;

namespace TISA.Services
{
    internal class PlayerService : IPlayerService
    {
        public Player Player { get; private set; }

        public async Task<Guid> CreatePlayerNameAsync(string playerName)
        {
            var response = await "http://localhost:7400/Player".PostJsonAsync(playerName);
            if (response.StatusCode != System.Net.HttpStatusCode.Created)
            {
                throw new InvalidOperationException("Something went wrong trying to create the player");
            }

            var player = await $"http://localhost:7400{response.Headers.Location}".GetJsonAsync<Player>();
            return player.Id;
        }

        public async Task SetPlayerByPlayerIdAsync(Guid playerId)
        {
            var response = await $"http://localhost:7400/Player/{playerId}".GetJsonAsync<Player>();
            Player = response;
        }

    }
}