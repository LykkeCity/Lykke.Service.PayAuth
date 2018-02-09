using AutoMapper;
using Lykke.Service.PayAuth.Models;
using Lykke.Service.PayAuth.Core.Domain;
using Lykke.Service.PayAuth.Models.Employees;

namespace Lykke.Service.PayAuth.Assets.AutoMapperProfiles
{
    public class DefaultProfile : Profile
    {
        public DefaultProfile()
        {
            CreateMap<PayAuthModel, Core.Domain.PayAuth>();

            CreateMap<RegisterModel, EmployeeCredentials>(MemberList.Destination)
                .ForMember(dest => dest.Salt, opt => opt.Ignore());
        }

        public override string ProfileName => "Default profile";
    }
}
