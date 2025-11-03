using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POCOTodoCross
{
    public class TaskStorage : ITaskStorage
    {
        public string name { get; set; }
        public TaskStorage()
        {
            name = "Default Task Storage";
        }
        private readonly Dictionary<Guid, Task> tasks = new();
        public void AddTask(Task task)
        {
            tasks[task.id] = task;
        }

        public void RemoveTask(Guid id)
        {
            tasks.Remove(id);
        }
        public Task GetTask(Guid id)
        {
            tasks.TryGetValue(id, out var task);
            return task;
        }
        public List<Task> GetAllTasks()
        {
            return tasks.Values.ToList();
        }
    }
}