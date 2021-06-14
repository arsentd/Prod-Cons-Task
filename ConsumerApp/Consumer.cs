using System;
using System.Threading;
using TaskDataProvider.Model;

namespace ConsumerApp
{
    public class Consumer
    {
        public Guid Id { get; private set; }

        public Consumer()
        {
            Id = Guid.NewGuid();
        }

        public void Consume(Task task)
        {
            Thread.Sleep(3000);
            Console.WriteLine($"Task '{task.TaskText}' was consumed by {Id}");
        }
    }
}