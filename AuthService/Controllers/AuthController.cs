using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuthService.DbModel;
using AuthService.Dto;
using AuthService.Security;
using AuthService.services;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NLog;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {


        private readonly Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private IUsersRepository _usersRepository;
        private ISessionRepository _sessionRepository;
        IMapper _mapper;
        public AuthController(IUsersRepository usersRepository, IMapper mapper, ISessionRepository sessionRepository)
        {
            _usersRepository = usersRepository;
            _mapper = mapper;
            _sessionRepository = sessionRepository;
        }

        [HttpPost]
        public UserDto Login([FromBody] LoginDto loginDto)
        {
            if (loginDto == null)
                throw new UnauthorizedAccessException();

            var user = _usersRepository.GetUserByUsername(loginDto.Username);
            if (user == null)
                throw new UnauthorizedAccessException();

            if (!user.ValidatePassword(loginDto.Password))
                throw new UnauthorizedAccessException();

            var tokenDto = _mapper.Map<TokenDto>(user);
            tokenDto.SessionId = _sessionRepository.NewSession(user.Id);
            tokenDto.ExpirationTime = DateTime.UtcNow.AddMinutes(AuthFilter.SessionLength);
            this.Response.Cookies.Append(AuthFilter.TokenHeader, JwtHelper.Encode(tokenDto), new CookieOptions() { Secure = true, HttpOnly = true });

            return _mapper.Map<UserDto>(user);
        }
    }
}
