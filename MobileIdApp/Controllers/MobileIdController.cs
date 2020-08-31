using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using Dto;
using Microsoft.AspNetCore.Mvc;
using MobileId.MobileId;
using MobileIdApp.mobileId;

namespace MobileIdApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MobileIdController : ControllerBase
    {
        private IMobileIdService _mobileIdService;
        private IMapper _mapper;

        public MobileIdController(IMobileIdService mobileIdService, IMapper mapper)
        {
            _mobileIdService = mobileIdService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("start")]
        public MobileIdDto Start(Languages language, string personalId, string number)
        {
            var mobileIdResponse = _mobileIdService.StartLogin(personalId, number, language, out string verificationCode);
            var mobileIdDto = new MobileIdDto();
            mobileIdDto.Code = verificationCode;
            mobileIdDto.SessionId = mobileIdResponse.sessionID;
            return mobileIdDto;
        }

        [HttpGet]
        [Route("check")]
        public UserDto Check(string sessionId)
        {
            var mobileIdResponse = _mobileIdService.CheckSession(sessionId);
            if (mobileIdResponse == null)
            {
                return null;
            }

            var bytes = Convert.FromBase64String(mobileIdResponse.cert);
            X509Certificate cert = new X509Certificate(bytes);
            var subject = cert.Subject.Split(',');
            string fname = null, lname = null, personalId = null;
            foreach (var part in subject)
            {
                if (part.Contains("SERIALNUMBER="))
                {
                    personalId = part.Split('=').Last();
                }
                if (part.Contains("G="))
                {
                    fname = part.Split('=').Last();
                }
                if (part.Contains("SN="))
                {
                    lname = part.Split('=').Last();
                }
            }

            return _mapper.Map<UserDto>(null);
        }
    }
}