using System;

namespace PROG6221.Services
{
    public class ChatbotService
    {
        private readonly CybersecurityChatbot _chatbot = new CybersecurityChatbot();

        public string ProcessMessage(string message)
        {
            return _chatbot.ProcessMessage(message);
        }

        public event Action<string> MessageReceived;

        public void SimulateTyping(string message)
        {
            MessageReceived?.Invoke($"Bot is typing...");
            System.Threading.Thread.Sleep(1000); // Simulate delay
            var response = _chatbot.ProcessMessage(message);
            MessageReceived?.Invoke($"Bot: {response}");
        }
    }
}