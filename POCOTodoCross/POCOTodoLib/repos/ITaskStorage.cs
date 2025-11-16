using System.Collections.Generic;

namespace POCOTodoCross.Models
{
    public interface ITaskStorage
    {
        void AddTask(ITask task);
        void RemoveTask(string id);
        ITask? GetTask(string id);
        void UpdateTask(ITask task);
        IEnumerable<ITask> GetAllTasks();
    }
}
