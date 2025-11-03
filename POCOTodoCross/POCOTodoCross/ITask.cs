using Microsoft.VisualBasic;

namespace POCOTodoCross
{
    public interface ITask
    {
        string id { get; set; }
        string title { get; set; }
        bool isCompleted { get; set; }
        string description { get; set; }
        DueDate dueDate { get; set; }
    }
}