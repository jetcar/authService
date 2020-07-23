using System;
using AuthService.services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AuthService.Security
{
    public class AuthFilter : IAuthorizationFilter
    {
        public static string TokenHeader = "token";
        public static string SessionData = "sessionData";

        private readonly ISessionRepository _sessionRepository;
        private IUsersRepository _usersRepository;

        public AuthFilter(ISessionRepository sessionRepository, IUsersRepository usersRepository)
        {
            _sessionRepository = sessionRepository;
            _usersRepository = usersRepository;
        }

        public static int SessionLength = 15;


        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var cookie = context.HttpContext.Request.Cookies[TokenHeader];
            if (cookie == null)
            {
                context.Result = new ForbidResult();
                return;
            }

            var tokenDto = JwtHelper.Decode<TokenDto>(cookie);
            if (tokenDto == null)
            {
                context.Result = new ForbidResult();
                return;
            }
            if (tokenDto.ExpirationTime < DateTime.UtcNow)
            {
                context.Result = new ForbidResult();
                return;
            }
            if (!_sessionRepository.CheckSession(tokenDto.SessionId))
            {
                context.Result = new ForbidResult();
                return;
            }

            var userDb = _usersRepository.GetUserByUsernameAndPassword(tokenDto.Username, tokenDto.Password);
            if (userDb == null)
            {
                context.Result = new ForbidResult();
                return;
            }
            context.HttpContext.Items[SessionData] = userDb;

        }
    }
}