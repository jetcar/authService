using System;
using AuthService.services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AuthService.Security
{
    public class AuthFilter : IAuthorizationFilter
    {
        public static string TokenHeader = "token";

        private readonly ISessionRepository _sessionRepository;
        private readonly IUsersRepository _usersRepository;
        private SessionDto _sessionDto;

        public AuthFilter(ISessionRepository sessionRepository, IUsersRepository usersRepository, SessionDto sessionDto)
        {
            _sessionRepository = sessionRepository;
            _usersRepository = usersRepository;
            _sessionDto = sessionDto;
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

            var tokenDto = JwtHelper.Decode<SessionDto>(cookie);
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
            if (!_sessionRepository.CheckSession(tokenDto.SessionId, tokenDto.UserId))
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

            _sessionDto.Username = tokenDto.Username;
            _sessionDto.ExpirationTime = tokenDto.ExpirationTime;
            _sessionDto.Password = tokenDto.Password;
            _sessionDto.SessionId = tokenDto.SessionId;
            _sessionDto.UserId = tokenDto.UserId;
        }
    }
}