using System;
using System.Text.RegularExpressions;

namespace POCOTodoCross.Models
{
    public class RecurringTask : Task
    {
        public required string RecurrencePattern { get; set; }
        public DateTime NextOccurrence { get; set; }

        public void UpdateNextOccurrence()
        {
            NextOccurrence = ComputeNextOccurrence(DateTime.Today);
        }

        /// <summary>
        /// detemines the next occurrence based on the recurrence pattern and a reference date.
        /// will support: 'Daily', 'Weekly', 'Every N days', 'Every N weeks'.
        /// </summary>
        public DateTime ComputeNextOccurrence(DateTime referenceDate)
        {
            string pattern = RecurrencePattern?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(pattern))
                return referenceDate.AddDays(1);

            // Case-insensitive matching
            pattern = pattern.ToLower();

            if (pattern == "daily")
                return referenceDate.AddDays(1);

            if (pattern == "weekly")
                return referenceDate.AddDays(7);

            // Match "Every N days" or "Every N Weeks"
            var match = Regex.Match(pattern, @"every\s+(\d+)\s+(day|week)s?", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                int count = int.Parse(match.Groups[1].Value);
                string unit = match.Groups[2].Value.ToLower();
                if (unit == "day")
                    return referenceDate.AddDays(count);
                if (unit == "week")
                    return referenceDate.AddDays(count * 7);
            }

            // Default: add 1 day if pattern doesn't match any known format
            return referenceDate.AddDays(1);
        }
    }
}
