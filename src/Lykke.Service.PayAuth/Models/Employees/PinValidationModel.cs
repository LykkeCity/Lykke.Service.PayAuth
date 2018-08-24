using System.ComponentModel.DataAnnotations;
using LykkePay.Common.Validation;

namespace Lykke.Service.PayAuth.Models.Employees
{
    public class PinValidationModel
    {
        [Required]
        [EmailAndRowKey]
        public string Email { get; set; }

        [Required]
        public string Pin { get; set; }
    }
}
