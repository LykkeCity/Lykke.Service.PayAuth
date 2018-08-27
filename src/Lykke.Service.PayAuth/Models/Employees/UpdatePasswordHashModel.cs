using System.ComponentModel.DataAnnotations;
using LykkePay.Common.Validation;

namespace Lykke.Service.PayAuth.Models.Employees
{
    public class UpdatePasswordHashModel
    {
        [Required]
        [EmailAndRowKey]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }
    }
}
