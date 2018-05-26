namespace Lykke.Service.PayAuth.Models.Employees
{
    public class CredentialsValidationResultModel
    {
        public CredentialsValidationResultModel()
        {
        }

        public CredentialsValidationResultModel(bool success)
        {
            Success = success;
        }
        
        public bool Success { get; set; }
        
        public string EmployeeId { get; set; }
        
        public string MerchantId { get; set; }

        public bool UpdatePasswordMarker { get; set; }
    }
}
