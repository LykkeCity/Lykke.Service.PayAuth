using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.PayAuth.Models.Employees
{
    public class EnforcePasswordUpdateModel
    {
        [Required]
        [Validation.EmailAddressAndRowKey]
        public string Email { get; set; }
    }
}
