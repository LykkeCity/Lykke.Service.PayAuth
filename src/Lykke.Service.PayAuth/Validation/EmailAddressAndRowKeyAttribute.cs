using System;
using System.ComponentModel.DataAnnotations;
using Common;

namespace Lykke.Service.PayAuth.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class EmailAddressAndRowKeyAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return value?.ToString().IsValidEmailAndRowKey() ?? false;
        }
    }
}
