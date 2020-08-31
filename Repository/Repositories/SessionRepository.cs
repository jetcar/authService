using System.Linq;
using Repository.DbModel;

namespace Repository.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        private MyDbContext _dbContext;

        public SessionRepository(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool CheckSession(long sessionId, int userId)
        {
            using (_dbContext.Database.BeginTransaction())
            {
                return _dbContext.Sessions.Any(x => x.Id == sessionId && x.UserId == userId && x.Active);
            }
        }

        public long NewSession(int userId)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                var sessionDb = new SessionDb(userId) { Active = true };
                _dbContext.Sessions.Add(sessionDb);
                _dbContext.SaveChanges();
                transaction.Commit();
                return sessionDb.Id;
            }
        }

        public void DeactivateSession(long sessionId, int userId)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                var sessionDb = _dbContext.Sessions.FirstOrDefault(x => x.Id == sessionId && x.UserId == userId);
                if (sessionDb != null)
                {
                    sessionDb.Active = false;
                }
                _dbContext.SaveChanges();
                transaction.Commit();
            }
        }
    }

    public interface ISessionRepository
    {
        bool CheckSession(long sessionId, int tokenDtoUserId);

        long NewSession(int userId);

        void DeactivateSession(long sessionId, int userId);
    }
}