using Microsoft.VisualBasic;

namespace POCOTodoCross
{
    public class Task : ITask, IAssignable
    {
        public string id { get; set; }
        public bool isCompleted { get; set; }
        public string description { get; set; }
        public DueDate dueDate { get; set; }
        public bool isAssigned { get; set; }

        public void ToggleCompleted() => isCompleted = !isCompleted;
        public void ToggleAssigned() => isAssigned = !isAssigned;

        public Task()
        {
            id = Guid.NewGuid().ToString();
            description = "";
            dueDate = new DueDate();
            isCompleted = false;
            isAssigned = false;
        }
        public Task(string description, DueDate dueDate)
        {
            this.id = Guid.NewGuid().ToString();
            this.description = description;
            this.dueDate = dueDate;
            this.isCompleted = false;
            this.isAssigned = false;
        }

    }
}