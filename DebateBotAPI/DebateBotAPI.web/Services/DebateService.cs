
using DebateBotAPI.web.Models;
using OpenAI;
using OpenAI.Chat;
using System.ClientModel;
using System.Text.Json;

namespace DebateBotAPI.web.Services
{
    public class DebateService
    {
        private readonly ChatClient _apiClient;

        public DebateService(string? apiKey)
        {
            this._apiClient = new ChatClient("gpt-4.1-nano", apiKey);
        }

        public async Task<DebateResponse> GetBotReply(DebateRequest request)
        {

            string? conversationId = request.conversation_id;

            if (string.IsNullOrEmpty(conversationId))
            {
                conversationId = Guid.NewGuid().ToString();
            }

            //Create the response object
            DebateResponse _response = new DebateResponse { conversation_id = conversationId };

            //Create messages list getting previous messages from file
            List<Models.MessageItem> _listPreviousMessages = Helpers.FileHelper.AppendMessageToConversation(conversationId, new Models.MessageItem
            {
                role = "user",
                message = request.message
            });

            //build messages for the AI, including prompt, previous and current message
            var messages = Helpers.AIHelper.BuilChatMessages(_listPreviousMessages);

            ClientResult response = await _apiClient.CompleteChatAsync(messages);
            BinaryData botreply = response.GetRawResponse().Content;

            using JsonDocument outputAsJson = JsonDocument.Parse(botreply.ToString());
            string? reply = outputAsJson.RootElement
                .GetProperty("choices"u8)[0]
                .GetProperty("message"u8)
                .GetProperty("content"u8)
                .GetString();

            if (string.IsNullOrEmpty(reply))
            {
                reply = "I'm sorry, I couldn't generate a response at this time.";
            }
            else
            {
                var _fullchat = Helpers.FileHelper.AppendMessageToConversation(conversationId, new Models.MessageItem
                {
                    role = "bot",
                    message = reply ?? string.Empty
                });

                reply = Newtonsoft.Json.JsonConvert.SerializeObject(_fullchat, new Newtonsoft.Json.JsonSerializerSettings
                {
                    NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore
                });
                _response.message = _fullchat;
            }

            return _response;
        }
    }
}
