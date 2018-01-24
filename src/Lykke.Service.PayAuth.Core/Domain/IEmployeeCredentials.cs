﻿namespace Lykke.Service.PayAuth.Core.Domain
{
    public interface IEmployeeCredentials
    {
        string EmployeeId { get; }
        
        string MerchantId { get; }
        
        string Email { get; }
        
        string Password { get; }
        
        string Salt { get; }
    }
}