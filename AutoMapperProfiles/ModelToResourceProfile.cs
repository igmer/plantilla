using System.Linq;
using AutoMapper;
using UniversalIdentity.Entities.Account;
using UniversalIdentity.Models;
using UniversalIdentity.Security.Tokens;
using UniversalIdentity.Resources.Account;
using UniversalIdentity.Resources.Roles;
using UniversalIdentity.Resources.Tokens;

namespace UniversalIdentity.AutoMapperProfiles
{
    public class ModelToResourceProfile : Profile
    {
        public ModelToResourceProfile()
        {
            CreateMap<ApplicationRole, RoleModel>();
            
            CreateMap<ApplicationUser, UserModel>();
            CreateMap<RegisterModel, ApplicationUser>();
            CreateMap<UpdateModel, ApplicationUser>();

            CreateMap<AccessToken, AccessTokenResource>()
                .ForMember(a => a.AccessToken, opt => opt.MapFrom(a => a.Token))
                .ForMember(a => a.RefreshToken, opt => opt.MapFrom(a => a.RefreshToken.Token))
                .ForMember(a => a.Expiration, opt => opt.MapFrom(a => a.Expiration));
        }
    }
}