using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Lykke.Service.PayAuth.Core.Domain;

namespace Lykke.Service.PayAuth.Services
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<IEmployeeCredentials, EmployeeCredentials>(MemberList.Destination);
        }
    }
}
