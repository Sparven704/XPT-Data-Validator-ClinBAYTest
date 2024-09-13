using System.Data;
using XPT_Data_Validator_ClinBAYTest.Interfaces;
using XPT_Data_Validator_ClinBAYTest.ValidationRules;

namespace XPT_Data_Validator_ClinBAYTest.Services
{
    public class DataValidatorService : IDataValidatorService
    {
        private readonly List<IValidationRule> _rules;

        public DataValidatorService()
        {
            _rules = new List<IValidationRule>
            {
                new EXDURValidationRule(),
                new LBSTRESCValidationRule()
            };
        }

        public Dictionary<string, Dictionary<string, List<int>>> ValidateDatasets(List<DataTable> datasets)
        {
            var validationResult = new Dictionary<string, Dictionary<string, List<int>>>();

            foreach (var dataset in datasets)
            {
                var datasetName = dataset.TableName;
                validationResult[datasetName] = new Dictionary<string, List<int>>();

                foreach (var rule in _rules)
                {
                    if (rule.IsApplicable(dataset))
                    {
                        var failedRows = rule.Validate(dataset);
                        validationResult[datasetName][rule.GetType().Name] = failedRows;
                    }
                }
            }

            return validationResult;
        }
    }
}
