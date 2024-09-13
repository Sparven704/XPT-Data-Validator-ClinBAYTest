using System.Data;
using XPTReader.Models;
using XPT_Data_Validator_ClinBAYTest.Interfaces;

namespace XPT_Data_Validator_ClinBAYTest.Services
{
    public class XptReaderService : IXptReaderService
    {
        public async Task<DataTable> LoadDatasetAsync(Stream stream, string tableName)
        {
            DataFile dataFile = await XPTReader.File.ReadDataFile(null, stream);
            DataTable table = dataFile.DataTables[0];
            table.TableName = tableName;
            return table;
        }
    }
}
