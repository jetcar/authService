using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthService.DbModel;

namespace AuthService.services
{
    public class SessionRepository : ISessionRepository
    {
        private MyDbContext _dbContext;

        public SessionRepository(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool CheckSession(long sessionId)
        {
            using (_dbContext.Database.BeginTransaction())
            {
                return _dbContext.Sessions.Any(x => x.Id == sessionId && x.Active);
            }
        }

        public long NewSession(int userId)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                var sessionDb = new SessionDb(userId){Active = true};
                _dbContext.Sessions.Add(sessionDb);
                _dbContext.SaveChanges();
                transaction.Commit();
                return sessionDb.Id;
            }
        }
    }

    public interface ISessionRepository
    {
        bool CheckSession(long sessionId);
        long NewSession(int userId);
    }
}
