using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Extensions;
using StatNeth.Blaise.API.DataRecord;

namespace Blaise.Api.Core.Extensions
{
    public static class DataRecordExtension
    {
        public static bool HasABetterOutcomeThan(this IDataRecord newDataRecord, IDataRecord existingDataRecord)
        {
            var sourceOutcome = newDataRecord.GetOutcomeCode();

            if (sourceOutcome == 0)
            {
                return false;
            }

            var comparisonOutcome = existingDataRecord.GetOutcomeCode();

            if (comparisonOutcome > 542)
            {
                return false;
            }

            return comparisonOutcome == 0 || sourceOutcome <= comparisonOutcome;
        }

        private static decimal GetOutcomeCode(this IDataRecord dataRecord)
        {
            return dataRecord.GetField(FieldNameType.HOut.FullName()).DataValue.IntegerValue;
        }
    }
}
