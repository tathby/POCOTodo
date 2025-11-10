namespace POCOTodoCross.Models
{
    public interface ITask
    {
        string id { get; set; }
        string title { get; set; }
        bool isCompleted { get; set; }
        string description { get; set; }
        DateTime? dueDate { get; set; }
        void ToggleCompleted();
    }
}
