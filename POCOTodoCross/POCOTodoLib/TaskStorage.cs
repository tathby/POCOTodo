using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POCOTodoCross
{
    public class TaskStorage : ITaskStorage
    {
        private readonly Dictionary<Guid, Task> tasks = new();
        public void AddTask(Task task)
        {
            throw new NotImplementedException();
        }
        public void RemoveTask(Guid id)
        {
            throw new NotImplementedException();
        }
        public Task GetTask(Guid id)
        {
            throw new NotImplementedException();
        }
        public List<Task> GetAllTasks()
        {
            throw new NotImplementedException();
        }
    }
}