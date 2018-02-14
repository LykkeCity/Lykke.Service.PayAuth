﻿using System.Threading.Tasks;
using Refit;
using Lykke.Service.PayAuth.Client.Models;

namespace Lykke.Service.PayAuth.Client.Api
{
    internal interface IPayAuthApi
    {
        [Post("/api/register")]
        Task RegisterAsync([Body] RegisterRequest request);

        [Post("/api/verify/signature")]
        Task<SignatureValidationResponse> VerifyAsync([Body] VerifyRequest request);
    }
}
