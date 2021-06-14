using System;
using TaskDataProvider.Model;
using TaskDataProvider;
using System.Threading;

namespace ProducerApp
{
    class Program
    {
        /*
         * Databse first approach.
         * Create a database.
         * Create a table.
         * CREATE TABLE [dbo].[Tasks] (
            [Id]               UNIQUEIDENTIFIER NOT NULL,
            [TaskText]         NVARCHAR (50)    NULL,
            [Status]           INT              NULL,
            [CreationTime]     DATETIME2 (7)    NOT NULL,
            [ModificationTime] DATETIME2 (7)    NOT NULL,
            [ConsumerId]       UNIQUEIDENTIFIER NULL,
            PRIMARY KEY CLUSTERED ([Id] ASC)
          );
         
            Set connection string in TestDbContext.cs file
         */
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
