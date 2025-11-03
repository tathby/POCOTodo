using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POCOTodoCross
{
    public class TaskList : IAssignable
    {
        public TaskList(int numTasks)
        {
            foreach (var i in Enumerable.Range(1, numTasks))
            {
                tasks.Add(new Task { id = Guid.NewGuid().ToString(), title = $"Task {i}", isAssigned = false });
            }
        }
    }
}