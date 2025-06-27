using System.Collections.Generic;
using System.Linq;

namespace PROG6221
{
    public static class SentimentAnalyzer
    {
        private static readonly HashSet<string> PositiveWords = new() { "thank", "thanks", "great", "good", "appreciate", "awesome", "cool" };
        private static readonly HashSet<string> NegativeWords = new() { "bad", "terrible", "useless", "hate", "annoyed", "angry", "frustrated" };
        private static readonly HashSet<string> ConcernWords = new() { "worried", "concerned", "scared", "nervous", "anxious", "uneasy" };

        public static string Analyze(string message)
        {
            string lowerMsg = message.ToLower();

            bool hasPositive = PositiveWords.Any(word => lowerMsg.Contains(word));
            bool hasNegative = NegativeWords.Any(word => lowerMsg.Contains(word));
            bool hasConcern = ConcernWords.Any(word => lowerMsg.Contains(word));

            if (hasConcern) return "concern";
            if (hasPositive && !hasNegative) return "positive";
            if (hasNegative && !hasPositive) return "negative";
            if (hasPositive && hasNegative) return "mixed";

            return "neutral";
        }
    }
}
