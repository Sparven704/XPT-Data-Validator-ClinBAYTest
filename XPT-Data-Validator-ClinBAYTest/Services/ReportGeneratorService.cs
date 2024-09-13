using OfficeOpenXml;
using System.Data;
using XPT_Data_Validator_ClinBAYTest.Interfaces;

namespace XPT_Data_Validator_ClinBAYTest.Services
{
    public class ReportGeneratorService : IReportGeneratorService
    {
        public void GenerateReport(Dictionary<string, Dictionary<string, List<int>>> validationResults, List<DataTable> datasets, Stream stream)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage package = new ExcelPackage())
            {
                // Add dataset sheets
                foreach (var dataset in datasets)
                {
                    var datasetName = dataset.TableName;
                    var worksheet = package.Workbook.Worksheets.Add($"{datasetName}_Data");

                    // Load dataset into worksheet
                    for (int i = 0; i < dataset.Columns.Count; i++)
                    {
                        worksheet.Cells[1, i + 1].Value = dataset.Columns[i].ColumnName;
                    }

                    for (int i = 0; i < dataset.Rows.Count; i++)
                    {
                        for (int j = 0; j < dataset.Columns.Count; j++)
                        {
                            worksheet.Cells[i + 2, j + 1].Value = dataset.Rows[i][j];
                        }
                    }
                }

                // Add a combined validation results sheet
                var validationSheet = package.Workbook.Worksheets.Add("Validation Results");

                int row = 1;
                validationSheet.Cells[row, 1].Value = "Dataset";
                validationSheet.Cells[row, 2].Value = "Rule";
                validationSheet.Cells[row, 3].Value = "Failed Row IDs";
                row++;

                // Add validation results for each dataset
                foreach (var datasetResult in validationResults)
                {
                    var datasetName = datasetResult.Key;

                    foreach (var ruleResult in datasetResult.Value)
                    {
                        validationSheet.Cells[row, 1].Value = datasetName; // Dataset name
                        validationSheet.Cells[row, 2].Value = ruleResult.Key; // Rule name
                        validationSheet.Cells[row, 3].Value = string.Join(", ", ruleResult.Value.Select(index => index + 2)); // Adjust indices by adding 2 to match the Excel row
                        row++;
                    }
                }

                package.SaveAs(stream);
            }
        }
    }
}