using System.Collections.Generic;
using System.Linq;
using Lykke.Common.Api.Contract.Responses;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Lykke.Service.PayAuth.Extensions
{
    public static class ErrorResponseExtensions
    {
        public static ErrorResponse AddErrors(this ErrorResponse errorResponse, ModelStateDictionary modelState)
        {
            errorResponse.ModelErrors = new Dictionary<string, List<string>>();

            foreach (var state in modelState)
            {
                if (state.Value.ValidationState == ModelValidationState.Valid)
                    continue;

                var messages = state.Value.Errors
                    .Where(e => !string.IsNullOrWhiteSpace(e.ErrorMessage))
                    .Select(e => e.ErrorMessage)
                    .Concat(state.Value.Errors
                        .Where(e => string.IsNullOrWhiteSpace(e.ErrorMessage))
                        .Select(e => e.Exception.Message))
                    .ToList();

                if (messages.Any()) 
                    errorResponse.ModelErrors.Add(state.Key, messages);
            }

            return errorResponse;
        }
    }
}
