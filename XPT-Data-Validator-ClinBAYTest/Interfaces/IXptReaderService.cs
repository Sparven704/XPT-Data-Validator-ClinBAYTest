using System.Data;

namespace XPT_Data_Validator_ClinBAYTest.Interfaces
{
    public interface IXptReaderService
    {
        Task<DataTable> LoadDatasetAsync(Stream stream, string tableName);
    }
}
