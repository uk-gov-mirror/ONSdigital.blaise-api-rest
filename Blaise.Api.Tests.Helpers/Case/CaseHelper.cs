using System;
using System.Collections.Generic;
using System.Globalization;
using Blaise.Api.Tests.Helpers.Configuration;
using Blaise.Api.Tests.Helpers.Enums;
using Blaise.Nuget.Api.Api;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Extensions;
using Blaise.Nuget.Api.Contracts.Interfaces;

namespace Blaise.Api.Tests.Helpers.Case
{
    public class CaseHelper
    {
        private readonly IBlaiseCaseApi _blaiseCaseApi;
        private int _primaryKey;

        private static CaseHelper _currentInstance;

        public CaseHelper()
        {
            _blaiseCaseApi = new BlaiseCaseApi();
            _primaryKey = 9000000;
        }

        public static CaseHelper GetInstance()
        {
            return _currentInstance ?? (_currentInstance = new CaseHelper());
        }

        public void CreateCasesInBlaise(int expectedNumberOfCases)
        {
            for (var count = 0; count < expectedNumberOfCases; count++)
            {
                CreateCaseInBlaise(_primaryKey.ToString());
                _primaryKey++;
            }
        }

        public void CreateCasesInFile(string extractedFilePath, int expectedNumberOfCases)
        {
            for (var count = 0; count < expectedNumberOfCases; count++)
            {
                CreateCaseInFile(extractedFilePath);
                _primaryKey++;
            }
        }

        public void CreateCaseInBlaise(string primaryKey, int outcome = 110, ModeType mode = ModeType.Tel)
        {
            var dataFields = new Dictionary<string, string>
            {
                { "SerialNumber", primaryKey },
                { FieldNameType.HOut.FullName(), outcome.ToString() },
                { FieldNameType.Mode.FullName(), mode.ToString() }
            };

            _blaiseCaseApi.CreateCase(primaryKey, dataFields,
                BlaiseConfigurationHelper.InstrumentName, BlaiseConfigurationHelper.ServerParkName);
        }

        public string CreateCaseInFile(string databaseFile, int outcome = 110, ModeType mode = ModeType.Web)
        {
            var dataFields = new Dictionary<string, string>
            {
                { "SerialNumber", _primaryKey.ToString() },
                { FieldNameType.HOut.FullName(), outcome.ToString() },
                { FieldNameType.Mode.FullName(), mode.ToString() }
            };

            _blaiseCaseApi.CreateCase(databaseFile, _primaryKey.ToString(), dataFields);
            return _primaryKey.ToString();
        }

        public void DeleteCases()
        {
            var cases = _blaiseCaseApi.GetCases(BlaiseConfigurationHelper.InstrumentName,
                BlaiseConfigurationHelper.ServerParkName);

            while (!cases.EndOfSet)
            {
                var primaryKey = _blaiseCaseApi.GetPrimaryKeyValue(cases.ActiveRecord);

                _blaiseCaseApi.RemoveCase(primaryKey, BlaiseConfigurationHelper.InstrumentName,
                    BlaiseConfigurationHelper.ServerParkName);

                cases.MoveNext();
            }
        }

        public int NumberOfCasesInInstrument()
        {
            return _blaiseCaseApi.GetNumberOfCases(BlaiseConfigurationHelper.InstrumentName,
                BlaiseConfigurationHelper.ServerParkName);
        }

        public object NumberOfCasesInInstrument(string dataInterfaceFile)
        {
            return _blaiseCaseApi.GetNumberOfCases(dataInterfaceFile);
        }

        public string GetMode(string primaryKey)
        {
            var field = _blaiseCaseApi.GetFieldValue(primaryKey, BlaiseConfigurationHelper.InstrumentName,
                BlaiseConfigurationHelper.ServerParkName, FieldNameType.Mode);
            var enumMode = field.EnumerationValue.ToString(CultureInfo.InvariantCulture);
            Enum.TryParse(enumMode, out ModeType modeType);
            return modeType.ToString();
        }
    }
}
