
using DebateBotAPI.web.Models;
using OpenAI;
using OpenAI.Chat;
using System.ClientModel;

namespace DebateBotAPI.web.Services
{
    public class DebateService
    {
        private string? _apiKey;

        public DebateService(string? apiKey)
        {
            this._apiKey = apiKey;
        }

        public async Task<string> GetBotReplyAsync(DebateRequest request)
        {
            return null;
        }
    }
}
