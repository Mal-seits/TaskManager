using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tasks.data
{
    public class TasksRepository
    {
        private readonly string _connectionString;
        public TasksRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public List<TaskItem> GetTaskItems(User user)
        {
            using var context = new TasksDbContext(_connectionString);
            var tasks = context.TaskItems.ToList();
            foreach(TaskItem task in tasks)
            {
                if(task.UserId == user.Id)
                {
                    task.IsThisUser = true;
                }
                else
                {
                    task.IsThisUser = false;
                }
            }
            return tasks;
        }
        public void AddTask(TaskItem taskItem)
        {
            using var context = new TasksDbContext(_connectionString);
            context.TaskItems.Add(taskItem);
            context.SaveChanges();
        }
        public void Update(TaskItem taskItem)
        {

            using var context = new TasksDbContext(_connectionString);
            context.TaskItems.Attach(taskItem);
            context.Entry(taskItem).State = EntityState.Modified;
            context.SaveChanges();
        }
        public void Delete(int taskId)
        {
            using var context = new TasksDbContext(_connectionString);
            context.Database.ExecuteSqlInterpolated($"DELETE FROM TaskItems WHERE Id = {taskId}");
        }
    }
}
