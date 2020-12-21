using System;
using System.Collections.Generic;
using System.Linq;

namespace Blaise.Api.Core.Extensions
{
    internal static class ArgumentValidationExtensions
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

        public static void ThrowExceptionIfNullOrEmpty(this IEnumerable<string> parameter, string parameterName)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(parameterName);
            }

            if (!parameter.Any())
            {

                throw new ArgumentException($"A value for the argument '{parameterName}' must be supplied");
            }
        }
    }
}