using System.Data;
using XPT_Data_Validator_ClinBAYTest.Interfaces;

namespace XPT_Data_Validator_ClinBAYTest.ValidationRules
{
    public class LBSTRESCValidationRule : IValidationRule
    {
        public bool IsApplicable(DataTable table)
        {
            return table.Columns.Contains("LBSTRESC") && table.Columns.Contains("LBDRVFL");
        }

        public List<int> Validate(DataTable table)
        {
            List<int> failedRows = new List<int>();

            foreach (DataRow row in table.Rows)
            {
                if (row["LBDRVFL"].ToString() == "Y" && string.IsNullOrEmpty(row["LBSTRESC"].ToString()))
                {
                    failedRows.Add(table.Rows.IndexOf(row));
                }
            }

            return failedRows;
        }
    }
}