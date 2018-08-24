using AutoMapper;
using Lykke.Service.PayAuth.Core.Domain;
using Lykke.Service.PayAuth.Models;
using Lykke.Service.PayAuth.Models.Employees;

namespace Lykke.Service.PayAuth.AutoMapperProfiles
{
    public class DefaultProfile : Profile
    {
        public DefaultProfile()
        {
            CreateMap<PayAuthModel, Core.Domain.PayAuth>();

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

            CreateMap<ResetPasswordAccessToken, ResetPasswordAccessTokenResponse>(MemberList.Destination)
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PublicId));
        }

        public override string ProfileName => "Default profile";
    }
}
