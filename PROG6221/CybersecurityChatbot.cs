using System;
using System.Collections.Generic;
using System.Linq;

namespace PROG6221
{
    public class CybersecurityChatbot
    {
        private readonly SecurityNlpProcessor _nlpProcessor = new();
        private readonly List<string> _activeReminders = new();
        private readonly List<string> _activeTasks = new();
        private readonly ResponseManager _responseManager = new();
        private readonly Random _random = new();

        public string ProcessMessage(string userMessage)
        {
            string sentiment = SentimentAnalyzer.Analyze(userMessage);
            var (handled, response) = _nlpProcessor.ProcessSecurityInput(userMessage);

            if (sentiment == "concern")
            {
                var topic = DetectTopic(userMessage);
                if (!string.IsNullOrEmpty(topic))
                {
                    return $"😟 I understand you're feeling concerned about {topic}. Here's a tip:\n"
                         + $"{_responseManager.GetResponse(topic)}\n\n"
                         + $"💡 Additional advice: {_responseManager.FollowUpTips[topic][_random.Next(_responseManager.FollowUpTips[topic].Count)]}";
                }
            }

            if (handled)
            {
                if (response.Contains("REMINDER:"))
                    _activeReminders.Add(response);
                else if (response.Contains("TASK CREATED:"))
                    _activeTasks.Add(response);

                return AddSentimentTone(response, sentiment);
            }

            return AddSentimentTone(GetDefaultResponse(userMessage), sentiment);
        }

        private string AddSentimentTone(string response, string sentiment)
        {
            return sentiment switch
            {
                "positive" => $"{response}\n🙂 Glad you're feeling positive!",
                "negative" => $"{response}\n😟 Sorry to hear that. Let me try to help.",
                "mixed" => $"{response}\n😐 I sense mixed feelings. Let's work through this together.",
                _ => response
            };
        }

        private string GetDefaultResponse(string message) => message.ToLower() switch
        {
            var s when s.Contains("hello") || s.Contains("hi") => "Hello! I'm your security assistant. How can I help?",
            var s when s.Contains("reminders") => GetRemindersList(),
            var s when s.Contains("tasks") => GetTasksList(),
            var s when s.Contains("help") => GetHelpMessage(),
            _ => _responseManager.GetRandomErrorResponse()
        };

        private string GetRemindersList() =>
            _activeReminders.Count == 0 ? "No active security reminders." : "ACTIVE REMINDERS:\n" + string.Join("\n", _activeReminders);

        private string GetTasksList() =>
            _activeTasks.Count == 0 ? "No pending security tasks." : "PENDING TASKS:\n" + string.Join("\n", _activeTasks);

        private string GetHelpMessage() => @"SECURITY HELP:
- Reminders: 'Remind me to update my password'
- Tasks: 'Create a task to check firewall settings'
- Quiz: 'Start security quiz'
- Topics: Ask about 2FA, phishing, privacy
- View: 'Show my reminders' or 'List tasks'";

        private string DetectTopic(string message)
        {
            var topics = _responseManager.FollowUpTips.Keys;
            return topics.FirstOrDefault(topic => message.ToLower().Contains(topic));
        }
    }
}
