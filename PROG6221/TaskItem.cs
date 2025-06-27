using System;

namespace PROG6221
{
    public class TaskItem
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public DateTime? ReminderDate { get; set; }
        public bool IsCompleted { get; set; }

        public override string ToString()
        {
            return $"{Title} - {Description}" +
                   $"{(ReminderDate.HasValue ? $" (Reminder: {ReminderDate.Value.ToShortDateString()})" : "")}" +
                   $"{(IsCompleted ? " [Completed]" : "")}";
        }
    }
}