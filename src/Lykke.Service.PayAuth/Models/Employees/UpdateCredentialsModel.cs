using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.PayAuth.Models.Employees
{
    public class UpdateCredentialsModel
    {
        [Required]
        public string EmployeeId { get; set; }

        [Required]
        public string MerchantId { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
