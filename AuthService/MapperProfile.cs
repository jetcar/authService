using System;
using AuthService.Controllers;
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
            this.CreateMap<UserDb, SessionDto>()
                .ForMember(x => x.SessionId, c => c.Ignore())
                .ForMember(x => x.Password, c => c.MapFrom(e => e.PasswordHash))
                .ForMember(x => x.UserId, c => c.MapFrom(e => e.Id))
                .ForMember(x => x.ExpirationTime, c => c.Ignore())
                ;

            this.CreateMap<RegisterDto, UserDb>()
                .ForMember(x => x.Id, c => c.Ignore())
                .ForMember(x => x.PasswordSalt, c => c.MapFrom(e => Guid.NewGuid().ToString()))
                .ForMember(x => x.PasswordHash, c => c.Ignore())
                .ForMember(x => x.CreatedAt, c => c.Ignore())
                .ForMember(x => x.ModifiedBy, c => c.Ignore())
                .ForMember(x => x.ModifiedAt, c => c.Ignore())
                .ForMember(x => x.CreatedAt, c => c.Ignore())
                ;

            this.CreateMap<EditUserDto, UserDb>()
               .ForMember(x => x.Id, c => c.Ignore())
               .ForMember(x => x.PasswordSalt, c => c.MapFrom(e => Guid.NewGuid().ToString()))
               .ForMember(x => x.PasswordHash, c => c.Ignore())
               .ForMember(x => x.CreatedAt, c => c.Ignore())
               .ForMember(x => x.ModifiedBy, c => c.Ignore())
               .ForMember(x => x.ModifiedAt, c => c.Ignore())
               .ForMember(x => x.Username, c => c.Ignore())
               ;


        }
    }
}