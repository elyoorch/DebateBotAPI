namespace DebateBotAPI.web.Models
{
    public class DebateRequest
    {
        public string? conversation_id { get; set; }
        public required string message { get; set; }
    }
}
