using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POCOTodoCross
{
    public interface ITaskStorage
    {
        void AddTask(Task task);
        void RemoveTask(Guid id);
        Task GetTask(Guid id);
        List<Task> GetAllTasks();
    }
}