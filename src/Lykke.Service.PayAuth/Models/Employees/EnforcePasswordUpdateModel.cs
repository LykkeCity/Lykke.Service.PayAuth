using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.PayAuth.Models.Employees
{
    public class EnforcePasswordUpdateModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
