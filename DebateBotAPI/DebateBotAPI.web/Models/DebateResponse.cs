using static DebateBotAPI.Models.APIModels;

namespace DebateBotAPI.web.Models
{
    public class DebateResponse
    {
        public string? conversation_id { get; set; }
        public List<MessageItem>? message { get; set; }
    }
}
