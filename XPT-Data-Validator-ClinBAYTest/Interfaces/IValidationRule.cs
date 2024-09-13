using System.Data;

namespace XPT_Data_Validator_ClinBAYTest.Interfaces
{
    public interface IValidationRule
    {
        bool IsApplicable(DataTable table);
        List<int> Validate(DataTable table);
    }
}
