using OpenAI.Chat;

namespace DebateBotAPI.web.Helpers
{
    public class AIHelper
    {
        public static List<ChatMessage> BuilChatMessages(List<Models.MessageItem> _listPreviousMessages)
        {
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
            return messages;
        }
    }
}
