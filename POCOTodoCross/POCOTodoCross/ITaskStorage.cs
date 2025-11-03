using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POCOTodoCross
{
    public interface ITaskStorage
    {
        public string name { get; set; }
        public void AddTask(Task task);
        public void RemoveTask(Guid id);
        public Task GetTask(Guid id);
        public List<Task> GetAllTasks();
    }
}