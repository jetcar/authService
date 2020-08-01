using System;
using AuthService.DbModel;
using AuthService.Dto;
using AuthService.Security;
using AuthService.services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
        public UserDto UserProfile()
        {
            var user = _usersRepository.GetUserById(_sessionDto.UserId);
            if (user == null)
                throw new UnauthorizedAccessException();


            return _mapper.Map<UserDto>(user);
        }
        [HttpPost]
        public UserDto UpdateUserProfile([FromBody] EditUserDto userDto)
        {
            var user = _usersRepository.UpdateUser(_sessionDto.UserId,userDto);
            if (user == null)
                throw new UnauthorizedAccessException();


            return _mapper.Map<UserDto>(user);
        }
    }
}