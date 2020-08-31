using System;
using System.Linq;
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
        [Route("signup")]
        public UserDto Register(RegisterDto userData)
        {
            var user = _usersRepository.AddNewUser(userData);

            return _mapper.Map<UserDto>(user);
        }
    }
}