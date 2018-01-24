﻿namespace Lykke.Service.PayAuth.Models.Employees
{
    public class ValidateResultModel
    {
        public ValidateResultModel()
        {
        }

        public ValidateResultModel(bool success)
        {
            Success = success;
        }
        
        public bool Success { get; set; }
        
        public string EmployeeId { get; set; }
        
        public string MerchantId { get; set; }
    }
}