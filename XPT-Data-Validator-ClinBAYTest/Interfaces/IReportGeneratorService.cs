using System.Data;

namespace XPT_Data_Validator_ClinBAYTest.Interfaces
{
    public interface IReportGeneratorService
    {
        void GenerateReport(Dictionary<string, Dictionary<string, List<int>>> validationResults, List<DataTable> datasets, Stream stream);
    }
}
