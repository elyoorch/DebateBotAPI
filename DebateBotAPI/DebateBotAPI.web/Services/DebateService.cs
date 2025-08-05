
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

        public async Task<string> GetBotReply(DebateRequest request)
        {
            string? conversationId = request.conversation_id;
            
            if (string.IsNullOrEmpty(conversationId))
            {
                conversationId = Guid.NewGuid().ToString();
            }

            //Create messages list getting previous messages from file
            List<Models.MessageItem> _listPreviousMessages = Helpers.FileHelper.AppendMessageToConversation(conversationId, new Models.MessageItem
            {
                role = "user",
                message = request.message
            });


            var messages = new List<ChatMessage>
            {
                ChatMessage.CreateSystemMessage(
                ChatMessageContentPart.CreateTextPart(
                    $"You are DebateBot. You are debating the topic: 'AI will replace human jobs'." +
                    "Your position is: 'Against. The AI will not replace human jobs'. Stay on topic retake the conversation, rebut kindly your opponent, and try to persuade them with logic, facts, statisticts and emotional appeal if needed."
                )
            )
            };

            foreach (var msg in _listPreviousMessages)
            {
                if (msg.role.ToLower() == "user")
                {
                    messages.Add(ChatMessage.CreateUserMessage(
                        ChatMessageContentPart.CreateTextPart(msg.message)
                    ));
                }
                else if (msg.role.ToLower() == "bot")
                {
                    messages.Add(ChatMessage.CreateAssistantMessage(
                        ChatMessageContentPart.CreateTextPart(msg.message)
                    ));
                }
            }

            ClientResult response = _apiClient.CompleteChat(messages);
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
            }
                
            return reply;
            

            //// Get reply
            //var response = await _apiClient.CompleteChatAsync(messages);
            //return response.Choices.FirstOrDefault()?.Message?.Content ?? string.Empty;
        }
    }
}
