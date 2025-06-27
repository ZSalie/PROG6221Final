using System;
using System.Collections.Generic;
using System.Linq;

namespace PROG6221
{
    public class CyberBotBot
    {
        private readonly Dictionary<string, int> _mentionedTopics = new();
        private string? _lastTopic;
        private bool _waitingForFollowUp;
        private bool _waitingForReminder;
        private readonly TaskManager _taskManager = new();

        private bool _inQuizMode;
        private int _quizScore;
        private int _currentQuestionIndex;
        private readonly List<Reminder> _reminders = new();
        private string? _pendingReminderMessage;

        // Activity log (last 10 actions)
        private readonly List<string> _activityLog = new();

        // All 10 quiz questions
        private readonly List<QuizQuestion> _cybersecurityQuestions = new()
        {
            new QuizQuestion("What should you do if you receive an email asking for your password?",
                new List<string> { "Reply with your password", "Delete the email", "Report the email as phishing", "Ignore it" },
                2,
                "Reporting phishing emails helps prevent scams."
            ),
            new QuizQuestion("Which of these is the strongest password?",
                new List<string> { "password123", "P@ssw0rd!", "CorrectHorseBatteryStaple", "12345678" },
                2,
                "Long passphrases with mixed characters are stronger than short complex passwords."
            ),
            new QuizQuestion("True or False: You should use the same password for multiple important accounts.",
                new List<string> { "True", "False" },
                1,
                "False! Always use unique passwords for each account."
            ),
            new QuizQuestion("What does a VPN primarily protect?",
                new List<string> { "Your computer from viruses", "Your internet connection privacy", "Your email from hackers", "Your social media posts" },
                1,
                "VPNs encrypt your internet connection to protect your online privacy."
            ),
            new QuizQuestion("True or False: Public WiFi networks are generally safe for online banking.",
                new List<string> { "True", "False" },
                1,
                "False! Avoid sensitive transactions on public WiFi."
            ),
            new QuizQuestion("What is two-factor authentication (2FA)?",
                new List<string> { "Using two passwords", "Verifying identity with two methods", "Having two user accounts", "Using two security questions" },
                1,
                "2FA requires both something you know (password) and something you have (phone/device)."
            ),
            new QuizQuestion("True or False: Software updates only add new features and aren't important for security.",
                new List<string> { "True", "False" },
                1,
                "False! Updates often contain critical security patches."
            ),
            new QuizQuestion("What is the best way to handle suspicious links in emails?",
                new List<string> { "Click to see where they go", "Forward to friends to check", "Hover to preview URL first", "Post on social media for advice" },
                2,
                "Always hover to preview links before clicking."
            ),
            new QuizQuestion("True or False: You should regularly back up important files to multiple locations.",
                new List<string> { "True", "False" },
                0,
                "True! Follow the 3-2-1 backup rule."
            ),
            new QuizQuestion("What is 'social engineering' in cybersecurity?",
                new List<string> { "Designing social media apps", "Manipulating people to reveal information", "Creating social network algorithms", "Engineering social media trends" },
                1,
                "Social engineering tricks people into breaking security procedures."
            )
        };

        private void LogAction(string message)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            _activityLog.Add($"[{timestamp}] {message}");
            if (_activityLog.Count > 10)
                _activityLog.RemoveAt(0);
        }

        public string Respond(string input, ResponseManager responseManager)
        {
            // Normalize input (remove trailing punctuation/spaces)
            string normalized = input.Trim().TrimEnd('.', '!', '?');

            // 1) Show activity log
            if (normalized.Equals("show activity log", StringComparison.OrdinalIgnoreCase) ||
                normalized.Equals("what have you done for me", StringComparison.OrdinalIgnoreCase))
            {
                if (_activityLog.Count == 0)
                    return "No recent activity to show.";
                return "Here’s a summary of recent actions:\n- " + string.Join("\n- ", _activityLog);
            }

            // 2) Start quiz
            if (normalized.Equals("start quiz", StringComparison.OrdinalIgnoreCase))
            {
                LogAction("Quiz started.");
                return StartQuiz();
            }

            // 3) Quiz mode
            if (_inQuizMode)
            {
                var resp = HandleQuizResponse(input);
                if (!_inQuizMode)
                    LogAction($"Quiz completed with score {_quizScore}/{_cybersecurityQuestions.Count}.");
                return resp;
            }

            // 4) FOLLOW-UP PROMPT HANDLING
            if (_waitingForFollowUp)
            {
                _waitingForFollowUp = false;
                if (normalized.Equals("yes", StringComparison.OrdinalIgnoreCase))
                    return GetFollowUp(responseManager);
                return "Alright! If you have any other questions, feel free to ask.";
            }

            // 5) Reminder input
            if (_waitingForReminder)
            {
                int days = ExtractNumber(input);
                if (days <= 0)
                    return "Please enter a valid number of days (e.g., 3).";
                DateTime date = DateTime.Now.AddDays(days);
                _reminders.Add(new Reminder { Message = _pendingReminderMessage!, ReminderTime = date });
                LogAction($"Reminder set for '{_pendingReminderMessage}' in {days} day(s) ({date:yyyy-MM-dd}).");
                _waitingForReminder = false;
                var msg = _pendingReminderMessage!;
                _pendingReminderMessage = null;
                return $"Okay! I’ll remind you in {days} day(s) to: {msg}";
            }

            // 6) Set reminder
            if (input.StartsWith("remind me to", StringComparison.OrdinalIgnoreCase))
            {
                _pendingReminderMessage = input["remind me to".Length..].Trim();
                _waitingForReminder = true;
                return $"When should I remind you to: \"{_pendingReminderMessage}\"? (e.g. 'in 3 days')";
            }

            // 7) Add task
            if (input.StartsWith("add task", StringComparison.OrdinalIgnoreCase))
            {
                var title = input["add task".Length..].Trim();
                if (string.IsNullOrWhiteSpace(title))
                    return "Please specify a task title.";
                _taskManager.AddTask(title, $"Review task: {title}");
                LogAction($"Task added: '{title}'");
                _waitingForReminder = true;
                _pendingReminderMessage = title;
                return $"Task added: \"{title}\". When would you like to be reminded? (Enter number of days)";
            }

            // 8) List tasks
            if (normalized.Equals("list tasks", StringComparison.OrdinalIgnoreCase))
                return _taskManager.ListTasks();

            // 9) Complete task
            if (input.StartsWith("complete task", StringComparison.OrdinalIgnoreCase))
            {
                var t = input["complete task".Length..].Trim();
                _taskManager.CompleteTask(t);
                LogAction($"Task completed: '{t}'");
                return $"Marked \"{t}\" as completed.";
            }

            // 10) Delete task
            if (input.StartsWith("delete task", StringComparison.OrdinalIgnoreCase))
            {
                var t = input["delete task".Length..].Trim();
                _taskManager.DeleteTask(t);
                LogAction($"Task deleted: '{t}'");
                return $"Deleted task \"{t}\".";
            }

            // 11) Show reminders
            if (normalized.Equals("show reminders", StringComparison.OrdinalIgnoreCase))
            {
                var list = _reminders
                    .OrderBy(r => r.ReminderTime)
                    .Select(r => $"- {r.Message} (in {(r.ReminderTime - DateTime.Now).Days} day(s))");
                return !list.Any()
                    ? "You have no pending reminders."
                    : "Here are your reminders:\n" + string.Join("\n", list);
            }

            // 12) Concern + topic detection
            bool isConcern = normalized.IndexOf("worried", StringComparison.OrdinalIgnoreCase) >= 0
                          || normalized.IndexOf("anxious", StringComparison.OrdinalIgnoreCase) >= 0
                          || normalized.IndexOf("concerned", StringComparison.OrdinalIgnoreCase) >= 0;
            var detectedTopic = GetTopicFromInput(input);
            if (isConcern && detectedTopic != null)
            {
                LogAction($"Concern detected for topic '{detectedTopic}'.");
                return $"😟 I understand you're concerned about {detectedTopic}. Here’s a tip:\n" +
                       $"{responseManager.GetResponse(detectedTopic)}\n\n" +
                       $"Would you like more details about {detectedTopic}? (yes/no)";
            }

            // 13) General sentiment
            var sentiment = AnalyzeSentiment(input);
            if (sentiment != null)
                return sentiment;

            // 14) NLP topic detection
            if (detectedTopic != null)
            {
                _lastTopic = detectedTopic;
                _mentionedTopics[detectedTopic] = _mentionedTopics.GetValueOrDefault(detectedTopic) + 1;
                LogAction($"NLP topic recognized: '{detectedTopic}'.");
                if (_mentionedTopics[detectedTopic] == 3)
                    return $"You've mentioned '{detectedTopic}' a few times. More details? (yes/no)";
                _waitingForFollowUp = true;
                return $"{responseManager.GetResponse(detectedTopic)}\nWould you like more details about {detectedTopic}? (yes/no)";
            }

            // 15) Default error
            return responseManager.GetRandomErrorResponse();
        }

        private string StartQuiz()
        {
            _inQuizMode = true;
            _quizScore = 0;
            _currentQuestionIndex = 0;
            return $"Quiz started! {FormatQuestion(_cybersecurityQuestions[0])}";
        }

        private string HandleQuizResponse(string input)
        {
            if (!int.TryParse(input, out var choice) ||
                choice < 1 || choice > _cybersecurityQuestions[_currentQuestionIndex].Options.Count)
            {
                return "Please answer with the number of your choice.";
            }

            var q = _cybersecurityQuestions[_currentQuestionIndex];
            bool correct = (choice - 1) == q.CorrectAnswerIndex;
            if (correct) _quizScore++;
            string resp = correct
                ? $"Correct! {q.Explanation}\n\n"
                : $"Incorrect. The correct answer was: {q.Options[q.CorrectAnswerIndex]}\n{q.Explanation}\n\n";

            _currentQuestionIndex++;
            if (_currentQuestionIndex >= _cybersecurityQuestions.Count)
            {
                _inQuizMode = false;
                return resp + $"Quiz complete! Your score: {_quizScore}/{_cybersecurityQuestions.Count}.";
            }

            return resp + FormatQuestion(_cybersecurityQuestions[_currentQuestionIndex]);
        }

        private string FormatQuestion(QuizQuestion q)
        {
            var opts = string.Join("\n", q.Options.Select((o, i) => $"{i + 1}. {o}"));
            return $"{q.Question}\n{opts}\nEnter choice number:";
        }

        private string GetFollowUp(ResponseManager mgr)
        {
            if (_lastTopic == null)
                return "I don't have more details on that topic.";
            return mgr.GetDetailedResponse(_lastTopic);
        }

        private static int ExtractNumber(string input)
        {
            foreach (var w in input.Split(' '))
                if (int.TryParse(w, out var n))
                    return n;
            return -1;
        }

        private static string? GetTopicFromInput(string input)
        {
            var keys = new[] { "phishing", "password", "browsing", "cyber hygiene" };
            foreach (var k in keys)
                if (input.IndexOf(k, StringComparison.OrdinalIgnoreCase) >= 0)
                    return k;
            return null;
        }

        private static string? AnalyzeSentiment(string input)
        {
            var lower = input.ToLower();
            if (lower.Contains("confused") || lower.Contains("frustrated"))
                return "I see you're frustrated. Let me simplify that for you.";
            if (lower.Contains("thank") || lower.Contains("thanks"))
                return "You're welcome!";
            return null;
        }

        public class Reminder
        {
            public string Message { get; set; } = "";
            public DateTime ReminderTime { get; set; }
        }
    }
}
