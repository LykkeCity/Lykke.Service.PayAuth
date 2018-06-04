using System.ComponentModel.DataAnnotations;
namespace Lykke.Service.PayAuth.Models.Employees
{
    public class PasswordValidationModel
    {
        [Required]
        [Validation.EmailAddressAndRowKey]
        public string Email { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}
