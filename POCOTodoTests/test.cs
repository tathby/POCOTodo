using System;
using System.Linq;
using Xunit;
using POCOTodoCross;
using PTask = POCOTodoCross.Task;
using POCOTodoCross.Models;
using POCOTodoCross.ViewModels;
using System.Collections.ObjectModel;

namespace POCOTodoTests
{
    public class TaskStorageTests
    {
        [Fact]
        public void AddTask_AssignsIdAndPersistsTask()
        {
            // Arrange
            var storage = new TaskStorage();
            var task = new PTask { description = "Buy milk", isCompleted = false };

            // Act
            storage.AddTask(task);

            Assert.NotEqual(Guid.Empty, task.id);
            var key = task.id;

            // Assert
            var fetched = storage.GetTask(key);
            Assert.NotNull(fetched);
            Assert.Equal(task.id, fetched.id);
            Assert.Equal("Buy milk", fetched.description);
            Assert.False(fetched.isCompleted);
        }

        [Fact]
        public void GetAllTasks_ReturnsAllAddedTasks()
        {
            // Arrange
            var storage = new TaskStorage();
            var t1 = new PTask { description = "A" };
            var t2 = new PTask { description = "B" };

            // Act
            storage.AddTask(t1);
            storage.AddTask(t2);

            var all = storage.GetAllTasks().ToList();

            // Assert
            Assert.Equal(2, all.Count);
            Assert.Contains(all, t => t.description == "A");
            Assert.Contains(all, t => t.description == "B");
        }

        [Fact]
        public void EditTask_ChangesFields()
        {
            // Arrange
            var storage = new TaskStorage();
            var task = new PTask { description = "Old", isCompleted = false };
            storage.AddTask(task);

            // mutate and re-add (storage uses id as key so AddTask can overwrite)
            task.description = "Updated";
            task.isCompleted = true;
            storage.AddTask(task);

            // Assert
            var fetched = storage.GetTask(task.id);
            Assert.Equal("Updated", fetched.description);
            Assert.True(fetched.isCompleted);
        }

        [Fact]
        public void RemoveTask_RemovesItem()
        {
            //arrange
            var storage = new TaskStorage();
            var task = new PTask { description = "ToDelete" };
            storage.AddTask(task);

            // act & assert
            var before = storage.GetAllTasks().ToList();
            Assert.Contains(before, t => t.id == task.id);

            storage.RemoveTask(task.id);

            var after = storage.GetAllTasks().ToList();
            Assert.DoesNotContain(after, t => t.id == task.id);
            Assert.Null(storage.GetTask(task.id));
        }

        [Fact]
        public void GetTask_ReturnsNullWhenNotFound()
        {
            // Arrange
            var storage = new TaskStorage();
            // Act
            var result = storage.GetTask(Guid.NewGuid());
            // Assert
            Assert.Null(result);
        }
    }

    public class ViewModelAndServiceTests
    {
        private class InMemoryTaskStorage : ITaskStorage
        {
            private readonly Dictionary<string, ITask> _tasks = new();
            public void AddTask(ITask task) => _tasks[task.id] = task;
            public void RemoveTask(string id) => _tasks.Remove(id);
            public ITask? GetTask(string id) => _tasks.TryGetValue(id, out var t) ? t : null;
            public IEnumerable<ITask> GetAllTasks() => _tasks.Values.ToList();
            public void UpdateTask(ITask task) => _tasks[task.id] = task;
        }

        [Fact]
        public void ViewModel_AddsAndCompletesTask()
        {
            // Arrange
            var storage = new InMemoryTaskStorage();
            var service = new TaskService(storage);
            var vm = new MainWindowViewModel(service);
            // Act & Assert
            vm.NewTaskTitle = "Test";
            vm.AddTaskCommand.Execute(null);
            Assert.Single(vm.Tasks);
            var task = vm.Tasks[0];
            Assert.False(task.isCompleted);
            vm.SelectedTask = task;
            vm.CompleteTaskCommand.Execute(null);
            Assert.True(task.isCompleted);
        }

        [Fact]
        public void ViewModel_OverdueFlagIsSet()
        {
            // Arrange
            var storage = new InMemoryTaskStorage();
            var service = new TaskService(storage);
            var vm = new MainWindowViewModel(service);
            // Act
            vm.NewTaskTitle = "Overdue";
            vm.NewTaskDueDate = DateTime.Today.AddDays(-2);
            vm.AddTaskCommand.Execute(null);
            var task = vm.Tasks[0];
            // Assert
            Assert.True(task.isOverdue);
            vm.SelectedTask = task;
            vm.CompleteTaskCommand.Execute(null);
            Assert.False(task.isOverdue); // completed task should not be overdue
        }

        [Fact]
        public void ViewModel_RecurringTaskReappears()
        {
            // Arrange
            var storage = new InMemoryTaskStorage();
            var service = new TaskService(storage);
            var vm = new MainWindowViewModel(service);
            // Act 
            vm.NewTaskTitle = "Recurring";
            vm.NewTaskDueDate = DateTime.Today;
            vm.IsRecurring = true;
            vm.NewTaskRecurrencePattern = "Daily";
            vm.AddTaskCommand.Execute(null);
            var task = vm.Tasks[0];
            vm.SelectedTask = task;
            vm.CompleteTaskCommand.Execute(null);
            // Assert
            Assert.True(task.isCompleted);
            vm.AdvanceDayCommand.Execute(null);
            Assert.False(task.isCompleted); // should reappear as incomplete
        }

        [Fact]
        public void ViewModel_EditTask_UpdatesFields()
        {
            // Arrange
            var storage = new InMemoryTaskStorage();
            var service = new TaskService(storage);
            var vm = new MainWindowViewModel(service);
            // Act 
            vm.NewTaskTitle = "EditMe";
            vm.NewTaskDescription = "desc";
            vm.AddTaskCommand.Execute(null);
            var task = vm.Tasks[0];
            vm.SelectedTask = task;
            vm.EditTaskCommand.Execute(null);
            vm.EditTaskTitle = "Edited";
            vm.EditTaskDescription = "changed";
            vm.EditTaskDueDate = DateTime.Today.AddDays(5);
            vm.SaveEditCommand.Execute(null);
            // Assert
            Assert.Equal("Edited", task.title);
            Assert.Equal("changed", task.description);
            Assert.Equal(DateTime.Today.AddDays(5), task.dueDate);
        }

        [Fact]
        public void TaskService_ToggleComplete_And_UpdateTask()
        {
            // Arrange
            var storage = new InMemoryTaskStorage();
            var service = new TaskService(storage);
            var task = new Task { id = Guid.NewGuid().ToString(), title = "A", description = "B" };
            // Act & Assert
            service.AddTask(task);
            Assert.False(task.isCompleted);
            service.ToggleCompleteTask(task.id);
            Assert.True(task.isCompleted);
            task.title = "Changed";
            service.UpdateTask(task);
            var fetched = storage.GetTask(task.id);
            Assert.Equal("Changed", fetched.title);
        }
    }
}