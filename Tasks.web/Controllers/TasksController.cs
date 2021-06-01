using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tasks.data;
using Tasks.web.ViewModels;

namespace Tasks.web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly IHubContext<ChatHub> _context;
        private readonly string _connectionString;
        private IConfiguration _configuration;

        public TasksController(IConfiguration configuration, IHubContext<ChatHub> context)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
            _configuration = configuration;
            _context = context;
        }

        [HttpGet]
        [Route("gettaskitems")]
        public List<TaskItem> GetTaskItems()
        {
            string userId = User.FindFirst("user")?.Value;
            var userRepo = new UserRepository(_connectionString);
            var currentUser = userRepo.GetUserByEmail(userId);
            var repo = new TasksRepository(_connectionString);
            return repo.GetTaskItems(currentUser);

        }
        [HttpPost]
        [Route("addtaskitem")]
        public void AddTaskItem(AddTaskViewModel vm)
        {

            var taskItem = new TaskItem
            {
                Name = vm.Name,
                Status = Status.Incomplete,
            };
            var repo = new TasksRepository(_connectionString);
            repo.AddTask(taskItem);
            _context.Clients.All.SendAsync("newtaskadded", GetTaskItems());

        }
        [HttpPost]
        [Route("updatetaskstatus")]
        public void UpdateTaskStatus(TaskItem taskItem)
        {
            string userId = User.FindFirst("user")?.Value;
            var userRepo = new UserRepository(_connectionString);
            var user = userRepo.GetUserByEmail(userId);
            taskItem.UserId = user.Id;
            var repo = new TasksRepository(_connectionString);
            repo.Update(taskItem);
            _context.Clients.All.SendAsync("statusChanged", GetTaskItems());
        }
        [HttpPost]
        [Route("markdone")]
        public void MarkDone(TaskItem taskItem)
        {
           
            var repo = new TasksRepository(_connectionString);
            repo.Delete(taskItem.Id);
            _context.Clients.All.SendAsync("statusChanged", GetTaskItems());
        }
    }
}
