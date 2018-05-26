using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.PayAuth.Models.Employees
{
    public class PinValidationModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Pin { get; set; }
    }
}
