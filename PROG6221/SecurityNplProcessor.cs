using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class SecurityNlpProcessor
{
    private readonly HashSet<string> _securityKeywords = new() { "password", "2fa", "phishing", "privacy", "authentication", "virus", "malware", "firewall" };
    private readonly HashSet<string> _actionKeywords = new() { "add", "create", "set", "remind", "update", "change", "enable", "disable" };
    private readonly HashSet<string> _taskTypeKeywords = new() { "task", "reminder", "quiz", "alert", "notification" };

    private readonly Regex _reminderPattern = new(@"(remind|add|create).*to (update|change|check|enable|disable).*(password|2fa|privacy|authentication|firewall)", RegexOptions.IgnoreCase);
    private readonly Regex _taskPattern = new(@"(add|create|set).*(task|reminder|alert).*(for|about|to)", RegexOptions.IgnoreCase);
    private readonly Regex _quizPattern = new(@"(start|take|begin).*(quiz|test)", RegexOptions.IgnoreCase);
    private readonly Regex _extractAction = new(@"(?<=to\s)(update|change|check|enable|disable)", RegexOptions.IgnoreCase);
    private readonly Regex _extractTarget = new(@"(password|2fa|privacy|authentication|firewall)", RegexOptions.IgnoreCase);

    public (bool Handled, string Response) ProcessSecurityInput(string userInput)
    {
        string lowerInput = userInput.ToLower();

        if (_reminderPattern.IsMatch(lowerInput))
        {
            return (true, CreateReminder(userInput));
        }

        if (_taskPattern.IsMatch(lowerInput))
        {
            return (true, CreateTask(userInput));
        }

        if (_quizPattern.IsMatch(lowerInput))
        {
            return (true, StartSecurityQuiz());
        }

        foreach (var keyword in _securityKeywords)
        {
            if (lowerInput.Contains(keyword))
            {
                return (true, HandleSecurityKeyword(keyword, lowerInput));
            }
        }

        return (false, string.Empty);
    }

    private string CreateReminder(string input)
    {
        var actionMatch = _extractAction.Match(input);
        var targetMatch = _extractTarget.Match(input);

        if (actionMatch.Success && targetMatch.Success)
        {
            return $"SECURITY REMINDER: {actionMatch.Value} your {targetMatch.Value} in 1 hour.";
        }

        return "I'll create a security reminder. Please specify what to remind you about (e.g., 'Remind me to update my password').";
    }

    private string CreateTask(string input)
    {
        var taskMatch = _taskPattern.Match(input);
        if (taskMatch.Success)
        {
            string taskDescription = ExtractTaskDescription(input);
            return $"SECURITY TASK CREATED: {taskDescription}";
        }
        return "Please specify your security task more clearly (e.g., 'Create a task to check firewall settings').";
    }

    private string StartSecurityQuiz()
    {
        return "SECURITY QUIZ STARTED: Question 1 - What's the most secure password practice?\nA) Using the same password everywhere\nB) Password manager with 2FA\nC) Writing them down";
    }

    private string HandleSecurityKeyword(string keyword, string input)
    {
        return keyword switch
        {
            "password" => "Password Tip: Use at least 12 characters with mixed types. Need a password generator?",
            "2fa" => "Two-Factor Authentication: Enable this in your security settings for all critical accounts.",
            "phishing" => "Phishing Alert: Never click links in unexpected emails. Verify sender addresses carefully.",
            "privacy" => "Privacy Settings: Review app permissions monthly and disable unnecessary data collection.",
            _ => $"Security Advisory: For {keyword} concerns, visit our security portal or contact IT support."
        };
    }

    private string ExtractTaskDescription(string input)
    {
        var toIndex = input.IndexOf(" to ", StringComparison.OrdinalIgnoreCase);
        var forIndex = input.IndexOf(" for ", StringComparison.OrdinalIgnoreCase);
        var aboutIndex = input.IndexOf(" about ", StringComparison.OrdinalIgnoreCase);

        int startIndex = Math.Max(toIndex, Math.Max(forIndex, aboutIndex));
        if (startIndex >= 0)
        {
            startIndex += input[startIndex] == 't' ? 4 : 5; // Skip "to " or "for/about "
            return input[startIndex..].TrimEnd('.', '!', '?');
        }

        return "Perform security review";
    }
}