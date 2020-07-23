using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthService.DbModel;

namespace AuthService.services
{
    public class UsersRepository : IUsersRepository
    {
        private MyDbContext _dbContext;

        public UsersRepository(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public UserDb GetUserByUsername(string username)
        {
            using (_dbContext.Database.BeginTransaction())
            {
                return _dbContext.Users.FirstOrDefault(x => x.Username == username);
            }
        }

        public UserDb GetUserByUsernameAndPassword(string username, string password)
        {
            using (_dbContext.Database.BeginTransaction())
            {
                return _dbContext.Users.FirstOrDefault(x => x.Username == username && x.PasswordHash == password);
            }
        }

        public UserDb GetUserById(int userId)
        {
            using (_dbContext.Database.BeginTransaction())
            {
                return _dbContext.Users.FirstOrDefault(x => x.Id == userId);
            }
        }
    }

    public interface IUsersRepository
    {
        UserDb GetUserByUsername(string username);
        UserDb GetUserByUsernameAndPassword(string username, string password);
        UserDb GetUserById(int userId);
    }
}
