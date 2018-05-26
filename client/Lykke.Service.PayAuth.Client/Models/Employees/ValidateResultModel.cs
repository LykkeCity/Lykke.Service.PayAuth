namespace Lykke.Service.PayAuth.Client.Models.Employees
{
    /// <summary>
    /// Represent a result of employee credentials validation.
    /// </summary>
    public class ValidateResultModel
    {
        /// <summary>
        /// Gets or sets a validation result.
        /// </summary>
        /// <remarks>
        /// If <c>true</c> the employee credential is valid; otherwise <c>false</c> 
        /// </remarks>
        public bool Success { get; set; }
        
        /// <summary>
        /// Gets or sets an employee id.
        /// </summary>
        ///<remarks>
        /// It will be <c>null</c> if <see cref="Success"/> is <c>false</c>.
        /// </remarks> 
        public string EmployeeId { get; set; }
        
        /// <summary>
        /// Gets or sets an merchant id.
        /// </summary>
        ///<remarks>
        /// It will be <c>null</c> if <see cref="Success"/> is <c>false</c>.
        /// </remarks> 
        public string MerchantId { get; set; }

        /// <summary>
        /// Gets or sets forcePasswordUpdate flag
        /// </summary>
        /// <remarks>
        /// Means that user password has to be changed
        /// </remarks>
        public bool ForcePasswordUpdate { get; set; }
    }
}
