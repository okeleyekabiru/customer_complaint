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

        }
    }
}
