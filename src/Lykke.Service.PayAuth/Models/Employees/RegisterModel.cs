using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.PayAuth.Models.Employees
{
    public class RegisterModel
    {
        [Required]
        public string EmployeeId { get; set; }
        
        [Required]
        public string MerchantId { get; set; }
        
        [Required]
        [Validation.EmailAddressAndRowKey]
        public string Email { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}
