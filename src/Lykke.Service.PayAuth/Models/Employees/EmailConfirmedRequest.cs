using System.ComponentModel.DataAnnotations;
using LykkePay.Common.Validation;

namespace Lykke.Service.PayAuth.Models.Employees
{
    public class EmailConfirmedRequest
    {
        [Required]
        [EmailAndRowKey]
        public string Email { get; set; }
    }
}
