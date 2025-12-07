using System.Collections.Generic;

namespace POCOTodoCross.Models
{
    public class TaskService
    {
        private readonly ITaskStorage _storage;

        public TaskService(ITaskStorage storage)
        {
            _storage = storage;
        }

        public void AddTask(ITask task)
        {
            _storage.AddTask(task);
        }

        public void RemoveTask(string id)
        {
            _storage.RemoveTask(id);
        }

        public void ToggleCompleteTask(string id)
        {
            var task = _storage.GetTask(id);
            if (task != null)
            {
                task.ToggleCompleted();
                _storage.UpdateTask(task);

                if (task is RecurringTask recurringTask && task.isCompleted)
                {
                    recurringTask.UpdateNextOccurrence();
                }
            }
        }

        public void EditTask(string id, string newDescription)
        {
            var task = _storage.GetTask(id);
            if (task != null)
            {
                task.description = newDescription;
                _storage.UpdateTask(task);
            }
        }

        public void UpdateTask(ITask task)
        {
            _storage.UpdateTask(task);
        }

        public IEnumerable<ITask> GetTasks()
        {
            return _storage.GetAllTasks();
        }
    }
}
