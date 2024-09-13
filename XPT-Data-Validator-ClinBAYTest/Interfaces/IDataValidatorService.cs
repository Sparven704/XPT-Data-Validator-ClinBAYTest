using System.Data;

namespace XPT_Data_Validator_ClinBAYTest.Interfaces
{
    public interface IDataValidatorService
    {
        Dictionary<string, Dictionary<string, List<int>>> ValidateDatasets(List<DataTable> datasets);
    }
}
