using Microsoft.VisualBasic;

namespace POCOTodoCross
{
    public class Task : ITask
    {
        public string id { get; set; }
        public string title { get; set; }
        public bool isCompleted { get; set; }
        public string description { get; set; }
        public DueDate dueDate { get; set; }

        public void ToggleCompleted()
        {
            isCompleted = !isCompleted;
        }
    }
}