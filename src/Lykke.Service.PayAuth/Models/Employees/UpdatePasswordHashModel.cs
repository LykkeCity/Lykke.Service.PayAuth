using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.PayAuth.Models.Employees
{
    public class UpdatePasswordHashModel
    {
        [Required]
        [Validation.EmailAddressAndRowKey]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }
    }
}
