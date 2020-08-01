using System;
using System.Linq;
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
    public class SignUpController : ControllerBase
    {
        private IUsersRepository _usersRepository;
        private IMapper _mapper;

        public SignUpController(IUsersRepository usersRepository, IMapper mapper)
        {
            _usersRepository = usersRepository;
            _mapper = mapper;
        }


        [HttpPost]
        public UserDto Register(RegisterDto userData)
        {
           
            var user =_usersRepository.AddNewUser(userData);

            return _mapper.Map<UserDto>(user);
        }
    }
}