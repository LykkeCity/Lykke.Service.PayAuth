using AutoMapper;
using Lykke.Service.PayAuth.AzureRepositories.ResetPasswordAccessToken;
using Lykke.Service.PayAuth.Core.Domain;

namespace Lykke.Service.PayAuth.AutoMapperProfiles
{
    public class AzureRepositoriesProfile : Profile
    {
        public AzureRepositoriesProfile()
        {
            CreateMap<ResetPasswordAccessToken, ResetPasswordAccessTokenEntity>(MemberList.Destination)
                .IgnoreTableEntityFields()
                .ForMember(src => src.Id, opt => opt.Ignore());

            CreateMap<ResetPasswordAccessTokenEntity, ResetPasswordAccessToken>(MemberList.Destination);
        }
    }
}
