using AuthService.DbModel;
using AuthService.Dto;
using AuthService.Security;
using AutoMapper.Configuration;

namespace AuthService
{
    public class MapperProfile : MapperConfigurationExpression
    {
        public MapperProfile()
        {
            this.CreateMap<UserDb, UserDto>().ReverseMap();
            this.CreateMap<UserDb, TokenDto>()
                .ForMember(x=>x.SessionId, c=>c.Ignore())
                .ForMember(x=>x.Password, c=>c.MapFrom(e=>e.PasswordHash))
                .ForMember(x=>x.UserId, c=>c.MapFrom(e=>e.Id))
                .ForMember(x=>x.ExpirationTime, c=>c.Ignore())
                ;
        }
    }
}