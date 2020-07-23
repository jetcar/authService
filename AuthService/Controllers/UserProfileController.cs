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

        public UserProfileController(IUsersRepository usersRepository, IMapper mapper)
        {
            _usersRepository = usersRepository;
            _mapper = mapper;
        }


        [HttpGet]
        public UserDto UserProfile()
        {
            var session = (UserDb)HttpContext.Items[AuthFilter.SessionData];
            var user = _usersRepository.GetUserById(session.Id);
            if (user == null)
                throw new UnauthorizedAccessException();


            return _mapper.Map<UserDto>(user);
        }
    }
}