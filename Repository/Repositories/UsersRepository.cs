using System;
using System.Linq;
using AutoMapper;
using CommonTools.services;
using Dto;
using Repository.DbModel;

namespace Repository.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly MyDbContext _dbContext;
        private readonly IMapper _mapper;

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
                var existingUser = _dbContext.Users.FirstOrDefault(x => x.Username == userData.Username);
                if (existingUser != null)
                    throw new ControllerException("Username", ErrorCodes.UsernameNotUnique);

                var newUser = _mapper.Map<UserDb>(userData);
                newUser.SetPassword(userData.Password);
                _dbContext.Users.Add(newUser);
                _dbContext.SaveChanges();
                transaction.Commit();
                return newUser;
            }
        }

        public UserDb UpdateUser(int userId, EditUserDto userDto)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                var existingUser = _dbContext.Users.FirstOrDefault(x => x.Id == userId);
                if (existingUser == null)
                    throw new UnauthorizedAccessException();

                _mapper.Map(userDto, existingUser);
                existingUser.SetPassword(userDto.NewPassword);
                _dbContext.SaveChanges();
                transaction.Commit();
                return existingUser;
            }
        }

        public UserDb GetOrCreateUserByPersonalId(string personalId, string firstName, string lastName)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                var existingUser = _dbContext.Users.FirstOrDefault(x => x.PersonalId == personalId);
                if (existingUser == null)
                {
                    existingUser = new UserDb()
                    {
                        PersonalId = personalId,
                        FirstName = firstName,
                        LastName = lastName
                    };
                    _dbContext.Users.Add(existingUser);
                    _dbContext.SaveChanges();
                    transaction.Commit();
                }
                return existingUser;
            }
        }
    }

    public interface IUsersRepository
    {
        UserDb GetUserByUsername(string username);

        UserDb GetUserByUsernameAndPassword(string username, string password);

        UserDb GetUserById(int userId);

        UserDb AddNewUser(RegisterDto userData);

        UserDb UpdateUser(int userId, EditUserDto userDto);

        UserDb GetOrCreateUserByPersonalId(string personalId, string firstName, string lastName);
    }
}