using System;
using TaskDataProvider.Model;
using TaskDataProvider;
using System.Threading;

namespace ProducerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var dbContext = new TestDbContext();

            for (int i = 0; i < 30; i++)
            {
                var task = new Task();
                task.TaskText = $"me task {i}";
                dbContext.Tasks.Add(task);
                dbContext.SaveChanges();
                Console.WriteLine($"Task '{task.TaskText}' was added to database");

                Thread.Sleep(2000);
            }

            Console.ReadKey();
        }
    }
}
