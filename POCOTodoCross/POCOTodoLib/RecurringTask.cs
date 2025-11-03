using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POCOTodoCross
{
    public class RecurringTask : Task
    {
        private string recurrencePattern { get; set; }
        private DateTime nextOccurrence { get; set; }
        public void UpdateNextOccurrence()
        {
            throw new NotImplementedException();
        }
    }
}