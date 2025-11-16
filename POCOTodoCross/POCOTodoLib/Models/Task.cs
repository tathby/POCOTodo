namespace POCOTodoCross.Models
{
    public class Task : ITask
    {
        public required string id { get; set; }
        public required string title { get; set; }
        public bool isCompleted { get; set; }
        public required string description { get; set; }
        public DateTime? dueDate { get; set; }

        public void ToggleCompleted()
        {
            isCompleted = !isCompleted;
        }
    }
}
