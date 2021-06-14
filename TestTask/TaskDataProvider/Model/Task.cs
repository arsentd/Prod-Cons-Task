using System;

namespace TaskDataProvider.Model
{
    public class Task
    {
        public Guid Id { get; set; }

        public string TaskText { get; set; }

        public TaskStatus Status { get; set; }

        public DateTime CreationTime { get; private set; }

        public DateTime ModificationTime { get; set; }

        public Guid ConsumerId { get; set; }

        public Task()
        {
            Status = TaskStatus.Pending;

            var date = DateTime.Now;
            CreationTime = date;
            ModificationTime = date;
        }
    }
}