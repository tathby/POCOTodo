namespace POCOTodoCross.Models
{
    public class RecurringTask : Task
    {
        public required string RecurrencePattern { get; set; }
        public DateTime NextOccurrence { get; set; }

        public void UpdateNextOccurrence()
        {
            // TODO: Implement recurrence pattern logic
            NextOccurrence = NextOccurrence.AddDays(1);
        }
    }
}
