using System;

namespace Tasks.data
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Status Status { get; set; }
        public int UserId { get; set; }
        public bool IsThisUser { get; set; }
    }
}
