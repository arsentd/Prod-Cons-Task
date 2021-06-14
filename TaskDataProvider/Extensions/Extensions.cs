using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskDataProvider.Model;

namespace TaskDataProvider.Extesions
{
    public static class Extensions
    {
        public static List<Task> GetLatestTasks(this DbSet<Task> tasks, List<Guid> customerIds)
        {
            return tasks.Where(t => customerIds.Contains(t.ConsumerId)).ToList();
        }

        public static int GetTasksCountOfStatus(this DbSet<Task> tasks, TaskStatus status)
        {
            return tasks.Where(t => t.Status == status).Count();
        }

        public static double GetSuccessAvgTime(this DbSet<Task> tasks)
        {
            if (tasks.Count() == 0)
            {
                return 0;
            }

            var totalTime = new TimeSpan(0);
            foreach (var task in tasks.Where(t => t.Status == TaskStatus.Done))
            {
                totalTime += task.ModificationTime - task.CreationTime;
            }
            return totalTime.TotalMilliseconds / tasks.Count();
        }

        public static double GetErrorsPercentage(this DbSet<Task> tasks)
        {
            if (tasks.Count() == 0)
            {
                return 0;
            }

            var errorCount = tasks.Where(t => t.Status == TaskStatus.Error).Count();
            return errorCount / tasks.Count() * 100;
        }
    }
}