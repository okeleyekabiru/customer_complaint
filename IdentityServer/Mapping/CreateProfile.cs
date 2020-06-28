using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using identity.Data.Model;
using IdentityServer.DTO;

namespace IdentityServer.Mapping
{
    public class CreateProfile:Profile
    {
        public CreateProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, RegisterDto>().ForMember(o => o.Password,p => p.Ignore() ).ReverseMap();
            CreateMap<RegisterDto, UserDto>().ReverseMap();
        }
    }
}
