using System.ComponentModel.DataAnnotations;
using LykkePay.Common.Validation;

namespace Lykke.Service.PayAuth.Models.Employees
{
    public class EnforceCredentialsUpdateModel
    {
        [Required]
        [EmailAndRowKey]
        public string Email { get; set; }
    }
}
