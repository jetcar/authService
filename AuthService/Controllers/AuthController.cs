﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuthService.Security;
using AutoMapper;
using Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NLog;
using Repository.Repositories;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private IUsersRepository _usersRepository;
        private ISessionRepository _sessionRepository;
        private IMapper _mapper;
        private SessionDto _sessionDto;

        public AuthController(IUsersRepository usersRepository, IMapper mapper, ISessionRepository sessionRepository, SessionDto sessionDto)
        {
            _usersRepository = usersRepository;
            _mapper = mapper;
            _sessionRepository = sessionRepository;
            _sessionDto = sessionDto;
        }

        [HttpPost]
        [Route("login")]
        public UserDto Login([FromBody] LoginDto loginDto)
        {
            if (loginDto == null)
                throw new UnauthorizedAccessException();

            var user = _usersRepository.GetUserByUsername(loginDto.Username);
            if (user == null)
                throw new UnauthorizedAccessException();

            if (!user.ValidatePassword(loginDto.Password))
                throw new UnauthorizedAccessException();

            var tokenDto = _mapper.Map<SessionDto>(user);
            tokenDto.SessionId = _sessionRepository.NewSession(user.Id);
            tokenDto.ExpirationTime = DateTime.UtcNow.AddMinutes(AuthFilter.SessionLength);
            this.Response.Cookies.Append(AuthFilter.TokenHeader, JwtHelper.Encode(tokenDto), new CookieOptions() { Secure = true, HttpOnly = true });

            return _mapper.Map<UserDto>(user);
        }

        [HttpGet]
        [AuthFilterRequirement]
        [Route("logout")]
        public bool Logout()
        {
            _sessionRepository.DeactivateSession(_sessionDto.SessionId, _sessionDto.UserId);
            this.Response.Cookies.Delete(AuthFilter.TokenHeader);

            return true;
        }
    }
}