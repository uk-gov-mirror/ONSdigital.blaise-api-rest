using System;

namespace Blaise.Api.Tests.Helpers.Extensions
{
    public static class ParameterValidationExtensions
    {
        public static void ThrowExceptionIfNullOrEmpty(this string parameter, string parameterName)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(parameterName);
            }

            if (string.IsNullOrWhiteSpace(parameter))
            {
                throw new ArgumentException($"A value for the argument '{parameterName}' must be supplied");
            }
        }
    }
}
