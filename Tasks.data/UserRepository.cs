using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tasks.data
{
    public class UserRepository
    {
        private readonly string _connectionString;
        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void AddUser(User user, string password)
        {
            var passwordHashed = BCrypt.Net.BCrypt.HashPassword(password);
            user.PasswordHash = passwordHashed;
            using var context = new TasksDbContext(_connectionString);
            context.Users.Add(user);
            context.SaveChanges();
        }
        public User GetUserByEmail(string email)
        {
            using var context = new TasksDbContext(_connectionString);
            return context.Users.FirstOrDefault(u => u.Email == email);
        }
        public User GetUserById(int id)
        {
            using var context = new TasksDbContext(_connectionString);
            var user = context.Users.FirstOrDefault(u => u.Id == id);
            return user;
        }
        public User Login(string email, string password)
        {
            var user = GetUserByEmail(email);
            if (user == null)
            {
                return null;
            }
            var isCorrectPassword = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            if (!isCorrectPassword)
            {
                return null;
            }
            return user;
        }
    }
}
