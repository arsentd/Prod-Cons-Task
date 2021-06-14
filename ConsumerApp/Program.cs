using System;
using TaskDataProvider;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskStatus = TaskDataProvider.Model.TaskStatus;
using Task = TaskDataProvider.Model.Task;
using System.Collections.Concurrent;
using TaskDataProvider.Extesions;

namespace ConsumerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter consumers count: ");
            if (int.TryParse(Console.ReadLine(), out int consumersCount))
            {
                if (consumersCount < 1)
                {
                    Console.Write("Need at least 1 consumer");
                    Console.ReadKey();
                    return;
                }
            }
            else
            {
                Console.Write("Consumers count must be a number");
                Console.ReadKey();
                return;
            }

            Console.Write("Enter tasks bulk size: ");
            if (int.TryParse(Console.ReadLine(), out int bulkSize))
            {
                if (bulkSize < 1)
                {
                    Console.Write("Task bulk size must be > 0");
                    Console.ReadKey();
                    return;
                }
            }
            else
            {

                Console.Write("Tasks bulk size must be a number");
                Console.ReadKey();
                return;
            }

            var dbContext = new TestDbContext();

            var consumers = new List<Consumer>();
            for (var i = 0; i < consumersCount; i++)
            {
                consumers.Add(new Consumer());
            }

            var tasks = dbContext.Tasks.Where(t => t.Status == TaskStatus.Pending).Take(bulkSize);
            while (tasks.Count() > 0)
            {
                var pendingTasks = new ConcurrentQueue<Task>(tasks);

                Parallel.ForEach(consumers, consumer =>
                {
                    if (pendingTasks.TryDequeue(out Task pendingTask))
                    {
                        var dbContext = new TestDbContext();
                        var taskToSave = dbContext.Tasks.FirstOrDefault(t => t.Id == pendingTask.Id);

                        taskToSave.Status = TaskStatus.InProgress;
                        taskToSave.ConsumerId = consumer.Id;
                        dbContext.SaveChanges();

                        try
                        {
                            consumer.Consume(taskToSave);
                            taskToSave.Status = TaskStatus.Done;
                        }
                        catch
                        {
                            taskToSave.Status = TaskStatus.Error;
                        }

                        taskToSave.ModificationTime = DateTime.Now;
                        dbContext.SaveChanges();
                    }
                });

                tasks = dbContext.Tasks.Where(t => t.Status == TaskStatus.Pending).Take(bulkSize);
            }

            Console.WriteLine("All tasks were consumed");

            Console.WriteLine($"Tasks in state of: {TaskStatus.Pending} are {dbContext.Tasks.GetTasksCountOfStatus(TaskStatus.Pending)}");
            Console.WriteLine($"Tasks in state of: {TaskStatus.InProgress} are {dbContext.Tasks.GetTasksCountOfStatus(TaskStatus.InProgress)}");
            Console.WriteLine($"Tasks in state of: {TaskStatus.Done} are {dbContext.Tasks.GetTasksCountOfStatus(TaskStatus.Done)}");
            Console.WriteLine($"Tasks in state of: {TaskStatus.Error} are {dbContext.Tasks.GetTasksCountOfStatus(TaskStatus.Error)}");

            Console.WriteLine($"Task avg. processing time: {dbContext.Tasks.GetSuccessAvgTime()}");

            Console.WriteLine($"Task % of errors: {dbContext.Tasks.GetErrorsPercentage()}");

            var consumerTasks = dbContext.Tasks.GetLatestTasks(new List<Guid>() { consumers.First().Id, consumers.Last().Id });
            Console.WriteLine($"Consumers {consumers.First().Id} and {consumers.Last().Id} latest tasks are:");
            foreach (var consumerTask in consumerTasks)
            {
                Console.WriteLine(consumerTask.TaskText);
            }

            Console.ReadKey();
        }
    }
}
