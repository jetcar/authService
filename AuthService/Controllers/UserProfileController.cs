using System;
using AuthService.Security;
using AutoMapper;
using Dto;
using Microsoft.AspNetCore.Mvc;
using Repository.Repositories;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AuthFilterRequirement]
    public class UserProfileController : ControllerBase
    {
        private IUsersRepository _usersRepository;
        private IMapper _mapper;
        private SessionDto _sessionDto;

        public UserProfileController(IUsersRepository usersRepository, IMapper mapper, SessionDto sessionDto)
        {
            _usersRepository = usersRepository;
            _mapper = mapper;
            _sessionDto = sessionDto;
        }

        [HttpGet]
        [Route("user")]
        public UserDto UserProfile()
        {
            var user = _usersRepository.GetUserById(_sessionDto.UserId);
            if (user == null)
                throw new UnauthorizedAccessException();

            return _mapper.Map<UserDto>(user);
        }

        [HttpPost]
        [Route("user")]
        public UserDto UpdateUserProfile([FromBody] EditUserDto userDto)
        {
            var user = _usersRepository.UpdateUser(_sessionDto.UserId, userDto);
            if (user == null)
                throw new UnauthorizedAccessException();

            return _mapper.Map<UserDto>(user);
        }
    }
}