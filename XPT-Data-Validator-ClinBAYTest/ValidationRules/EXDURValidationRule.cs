using System.Data;
using XPT_Data_Validator_ClinBAYTest.Interfaces;

namespace XPT_Data_Validator_ClinBAYTest.ValidationRules
{
    public class EXDURValidationRule : IValidationRule
    {
        public bool IsApplicable(DataTable table)
        {
            return table.Columns.Contains("EXDUR");
        }

        public List<int> Validate(DataTable table)
        {
            List<int> failedRows = new List<int>();

            foreach (DataRow row in table.Rows)
            {
                if (row["EXDUR"] != DBNull.Value)
                {
                    if (double.TryParse(row["EXDUR"].ToString(), out double exdurValue))
                    {
                        if (exdurValue < 0 && exdurValue > double.NegativeInfinity)
                        {
                            failedRows.Add(table.Rows.IndexOf(row));
                        }
                    }
                }
            }

            return failedRows;
        }
    }
}