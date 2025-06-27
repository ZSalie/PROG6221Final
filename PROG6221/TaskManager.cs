using System;
using System.Collections.Generic;
using System.Linq;

namespace PROG6221
{
    public class TaskManager
    {
        private readonly List<Task> _tasks = new();

        public void AddTask(string title, string description)
        {
            _tasks.Add(new Task { Title = title, Description = description, IsCompleted = false });
        }

        public void CompleteTask(string title)
        {
            var task = _tasks.FirstOrDefault(t => t.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
            if (task != null) task.IsCompleted = true;
        }

        public bool DeleteTask(string title)
        {
            var task = _tasks.FirstOrDefault(t => t.Title.Equals(title.Trim(), StringComparison.OrdinalIgnoreCase));
            if (task != null)
            {
                _tasks.Remove(task);
                return true;
            }
            return false;
        }

        public string ListTasks()
        {
            if (!_tasks.Any()) return "No tasks available.";
            var list = _tasks.Select(t => $"{t.Title} - {(t.IsCompleted ? "Completed" : "Pending")}");
            return "Tasks:\n" + string.Join("\n", list);
        }
    }

    public class Task
    {
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public bool IsCompleted { get; set; }
    }
}