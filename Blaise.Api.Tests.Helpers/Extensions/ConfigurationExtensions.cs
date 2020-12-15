using System;
using System.Configuration;

namespace Blaise.Api.Tests.Helpers.Extensions
{
    public class ConfigurationExtensions
    {
        public static string GetEnvironmentVariable(string variableName)
        {
            var variable = Environment.GetEnvironmentVariable(variableName) ?? GetVariable(variableName);
            variable.ThrowExceptionIfNullOrEmpty(variableName);
            return variable;
        }

        public static int GetIntEnvironmentVariable(string variableName)
        {
            var variable = Environment.GetEnvironmentVariable(variableName) ?? GetVariable(variableName);
            variable.ThrowExceptionIfNotInt(variableName);
            return Convert.ToInt32(variable);
        }

        public static string GetVariable(string variableName)
        {
            var variable = ConfigurationManager.AppSettings[variableName];
            variable.ThrowExceptionIfNullOrEmpty(variableName);
            return variable;
        }

        public static int GetIntVariable(string variableName)
        {
            var variable = ConfigurationManager.AppSettings[variableName];
            variable.ThrowExceptionIfNotInt(variableName);
            return Convert.ToInt32(variable);
        }
    }
}
