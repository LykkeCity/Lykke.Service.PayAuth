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
                .ForMember(dest => dest.Salt, opt => opt.Ignore())
                .ForMember(dest => dest.PinCode, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatePassword, opt => opt.UseValue(true));

            CreateMap<UpdateCredentialsModel, EmployeeCredentials>(MemberList.Destination)
                .ForMember(dest => dest.Salt, opt => opt.Ignore())
                .ForMember(dest => dest.PinCode, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatePassword, opt => opt.Ignore());
        }

        public override string ProfileName => "Default profile";
    }
}
