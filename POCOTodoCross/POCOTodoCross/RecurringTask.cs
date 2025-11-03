using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POCOTodoCross
{
    public class RecurringTask : Task
    {
        private string recurrencePattern { get; set; }
        private DateTime nextOccurrence { get; set; }
        public RecurringTask(string description, DueDate dueDate, string pattern, DateTime firstOccurrence)
        {
            this.id = Guid.NewGuid().ToString();
            this.description = description;
            this.dueDate = dueDate;
            this.recurrencePattern = pattern;
            this.nextOccurrence = firstOccurrence;
            this.isCompleted = false;
        }
        public void UpdateNextOccurrence(string pattern, DateTime newOccurrence)
        {
            recurrencePattern = pattern;
            nextOccurrence = newOccurrence;
        }
    }
}