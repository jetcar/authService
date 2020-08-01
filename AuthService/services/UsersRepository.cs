﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthService.Controllers;
using AuthService.DbModel;
using AutoMapper;

namespace AuthService.services
{
    public class UsersRepository : IUsersRepository
    {
        private MyDbContext _dbContext;
        private IMapper _mapper;

        public UsersRepository(MyDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
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

        public UserDb AddNewUser(RegisterDto userData)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                var existringUser = _dbContext.Users.FirstOrDefault(x => x.Username == userData.Username);
                if (existringUser != null)
                    throw new ControllerException("Username", ErrorCodes.UsernameNotUnique);

                var newUser = _mapper.Map<UserDb>(userData);
                newUser.SetPassword(userData.Password);
                _dbContext.Users.Add(newUser);
                _dbContext.SaveChanges();
                transaction.Commit();
                return newUser;
            }
        }
    }

    public interface IUsersRepository
    {
        UserDb GetUserByUsername(string username);
        UserDb GetUserByUsernameAndPassword(string username, string password);
        UserDb GetUserById(int userId);
        UserDb AddNewUser(RegisterDto userData);
    }
}
