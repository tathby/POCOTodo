using System.Collections.Generic;

namespace POCOTodoCross.Models
{
    public class TaskStorage : ITaskStorage
    {
        private readonly Dictionary<string, ITask> _tasks = new();

        public void AddTask(ITask task)
        {
            _tasks[task.id] = task;
        }

        public ITask? GetTask(string id)
        {
            return _tasks.TryGetValue(id, out var task) ? task : null;
        }

        public IEnumerable<ITask> GetAllTasks()
        {
            return _tasks.Values;
        }

        public void RemoveTask(string id)
        {
            _tasks.Remove(id);
        }

        public void UpdateTask(ITask task)
        {
            if (_tasks.ContainsKey(task.id))
            {
                _tasks[task.id] = task;
            }
        }
    }
}
