using AutoMapper;
using Lykke.Service.PayAuth.Models;
using Lykke.Service.PayAuth.Core.Domain;
using Lykke.Service.PayAuth.Models.Employees;
using Lykke.Service.PayAuth.Core;

namespace Lykke.Service.PayAuth.Assets.AutoMapperProfiles
{
    public class DefaultProfile : Profile
    {
        public DefaultProfile()
        {
            CreateMap<PayAuthModel, Core.Domain.PayAuth>(MemberList.Destination)
                .ForMember(dest => dest.SystemId, opt => opt.NullSubstitute(LykkePayConstants.DefaultSystemId));

            CreateMap<UpdateApiKeyModel, Core.Domain.PayAuth>(MemberList.Destination)
                .ForMember(dest => dest.SystemId, opt => opt.UseValue(LykkePayConstants.DefaultSystemId))
                .ForMember(dest => dest.Certificate, opt => opt.Ignore());

            CreateMap<RegisterModel, EmployeeCredentials>(MemberList.Destination)
                .ForMember(dest => dest.Salt, opt => opt.Ignore())
                .ForMember(dest => dest.PinCode, opt => opt.Ignore())
                .ForMember(dest => dest.ForcePasswordUpdate, opt => opt.UseValue(true))
                .ForMember(dest => dest.ForcePinUpdate, opt => opt.UseValue(true));

            CreateMap<UpdateCredentialsModel, EmployeeCredentials>(MemberList.Destination)
                .ForMember(dest => dest.Salt, opt => opt.Ignore())
                .ForMember(dest => dest.PinCode, opt => opt.Ignore())
                .ForMember(dest => dest.ForcePasswordUpdate, opt => opt.Ignore())
                .ForMember(dest => dest.ForcePinUpdate, opt => opt.Ignore());
        }

        public override string ProfileName => "Default profile";
    }
}
