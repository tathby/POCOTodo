using System;
using System.Linq;
using Xunit;
using POCOTodoCross;
using PTask = POCOTodoCross.Task;

namespace POCOTodoTests
{
    public class TaskStorageTests
    {
        [Fact]
        public void AddTask_AssignsIdAndPersistsTask()
        {
            var storage = new TaskStorage();
            var task = new PTask { description = "Buy milk", isCompleted = false };

            // Add to storage
            storage.AddTask(task);

            Assert.NotEqual(Guid.Empty, task.id);
            var key = task.id;

            var fetched = storage.GetTask(key);
            Assert.NotNull(fetched);
            Assert.Equal(task.id, fetched.id);
            Assert.Equal("Buy milk", fetched.description);
            Assert.False(fetched.isCompleted);
        }

        [Fact]
        public void GetAllTasks_ReturnsAllAddedTasks()
        {
            var storage = new TaskStorage();
            var t1 = new PTask { description = "A" };
            var t2 = new PTask { description = "B" };
            storage.AddTask(t1);
            storage.AddTask(t2);

            var all = storage.GetAllTasks().ToList();

            Assert.Equal(2, all.Count);
            Assert.Contains(all, t => t.description == "A");
            Assert.Contains(all, t => t.description == "B");
        }

        [Fact]
        public void EditTask_ChangesFields()
        {
            var storage = new TaskStorage();
            var task = new PTask { description = "Old", isCompleted = false };
            storage.AddTask(task);

            // mutate and re-add (storage uses id as key so AddTask can overwrite)
            task.description = "Updated";
            task.isCompleted = true;
            storage.AddTask(task);

            var fetched = storage.GetTask(task.id);
            Assert.Equal("Updated", fetched.description);
            Assert.True(fetched.isCompleted);
        }

        [Fact]
        public void RemoveTask_RemovesItem()
        {
            var storage = new TaskStorage();
            var task = new PTask { description = "ToDelete" };
            storage.AddTask(task);

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
            var storage = new TaskStorage();
            var result = storage.GetTask(Guid.NewGuid());
            Assert.Null(result);
        }
    }
}