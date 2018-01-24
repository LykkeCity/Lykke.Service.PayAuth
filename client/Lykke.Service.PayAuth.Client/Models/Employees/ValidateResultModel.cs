namespace Lykke.Service.PayAuth.Client.Models.Employees
{
    public class ValidateResultModel
    {
        public bool Success { get; set; }
        public string EmployeeId { get; set; }
        public string MerchantId { get; set; }
    }
}
