namespace DebateBotAPI.web.Helpers
{
    public class FileHelper
    {
        public static List<Models.MessageItem> AppendMessageToConversation(string conversationId, Models.MessageItem messageItem)
        {
            
            List<Models.MessageItem> messages = new List<Models.MessageItem>();
            string sfilename = $"{conversationId}.json";
            string sfilePath = GetFilePath(sfilename);

            if (File.Exists(sfilePath))
            {
                // Read the existing content of the file
                string scontent = File.ReadAllText(sfilePath);
                Newtonsoft.Json.JsonConvert.DeserializeObject<List<Models.MessageItem>>(scontent, new Newtonsoft.Json.JsonSerializerSettings
                {
                    NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore
                })?.ForEach(m => messages.Add(m));
            }

            messages.Add(new Models.MessageItem
            {
                role = messageItem.role,
                message = messageItem.message
            });
            File.WriteAllText(sfilePath, Newtonsoft.Json.JsonConvert.SerializeObject(messages));

            return messages;

        }
        public static string GetFilePath(string fileName)
        {
            if (!Directory.Exists(Directory.GetCurrentDirectory() + "/Chats"))
            { 
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/Chats");
            }
            // Ensure the file name is not null or empty
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentException("File name cannot be null or empty.", nameof(fileName));
            }
            // Combine the current directory with the file name
            return Path.Combine(Directory.GetCurrentDirectory() + "/Chats", fileName);
        }

    }
}
