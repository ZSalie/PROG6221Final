using System;
using System.Collections.Generic;

namespace PROG6221
{
    public class ResponseManager
    {
        private readonly Random _random = new Random();

        private readonly Dictionary<string, List<string>> _responses = new Dictionary<string, List<string>>
        {
            { "phishing",      new List<string> { "Avoid clicking unknown links.", "Phishing scams can trick users into giving personal information." } },
            { "password",      new List<string> { "Use strong passwords!", "Enable two-factor authentication for extra protection." } },
            { "browsing",      new List<string> { "Check for HTTPS!", "Be cautious of unknown sites." } },
            { "cyber hygiene", new List<string> { "Update your software!", "Enable automatic security updates." } }
        };

        private readonly Dictionary<string, List<string>> _followUpTips = new Dictionary<string, List<string>>
        {
            { "phishing",      new List<string> { "Verify senders before clicking links.", "Don't share sensitive info via email." } },
            { "password",      new List<string> { "Use passphrases.", "Change passwords regularly." } },
            { "browsing",      new List<string> { "Use a VPN.", "Clear your browser history." } },
            { "cyber hygiene", new List<string> { "Backup your data.", "Be careful with downloads." } }
        };

        private readonly List<string> _errorResponses = new List<string>
        {
            "I'm sorry, I didn't understand that.",
            "Can you rephrase your question?",
            "Let's try that again. What would you like to know?"
        };

        public string GetRandomErrorResponse()
            => _errorResponses[_random.Next(_errorResponses.Count)];

        public string GetResponse(string topic)
            => _responses.TryGetValue(topic, out var list)
                ? list[_random.Next(list.Count)]
                : "Topic not found.";

        /// <summary>
        /// Provides more detailed info for a given topic by joining all its responses.
        /// </summary>
        public string GetDetailedResponse(string topic)
        {
            if (!_responses.TryGetValue(topic, out var list))
                return "No detailed info available for that topic.";
            return string.Join(" ", list);
        }

        /// <summary>
        /// Exposes the follow-up tips dictionary so the bot can look up tips by topic.
        /// </summary>
        public Dictionary<string, List<string>> FollowUpTips => _followUpTips;
    }
}
