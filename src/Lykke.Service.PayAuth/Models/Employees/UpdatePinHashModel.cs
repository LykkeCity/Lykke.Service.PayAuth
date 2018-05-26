using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.PayAuth.Models.Employees
{
    public class UpdatePinHashModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PinHash { get; set; }
    }
}
