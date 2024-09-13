using System.Data;
using XPT_Data_Validator_ClinBAYTest.ValidationRules;

namespace ValidationRulesUnitTests
{
    public class LBSTRESCValidationRuleTests
    {
        private readonly LBSTRESCValidationRule _rule;

        public LBSTRESCValidationRuleTests()
        {
            _rule = new LBSTRESCValidationRule();
        }

        [Fact]
        public void IsApplicable_ReturnsFalse_WhenColumnsAreMissing()
        {
            var table = new DataTable();
            table.Columns.Add("SomeOtherColumn");

            var result = _rule.IsApplicable(table);

            Assert.False(result);
        }

        [Fact]
        public void IsApplicable_ReturnsTrue_WhenBothColumnsExist()
        {
            var table = new DataTable();
            table.Columns.Add("LBSTRESC");
            table.Columns.Add("LBDRVFL");

            var result = _rule.IsApplicable(table);

            Assert.True(result);
        }

        [Fact]
        public void Validate_ReturnsEmpty_WhenNoRowsMatchValidation()
        {
            var table = new DataTable();
            table.Columns.Add("LBSTRESC");
            table.Columns.Add("LBDRVFL");

            table.Rows.Add("Valid", "");
            table.Rows.Add("Value", "");

            var result = _rule.Validate(table);

            Assert.Empty(result);
        }

        [Fact]
        public void Validate_ReturnsFailedRows_WhenLBSTRESCIsEmptyAndLBDRVFLIsY()
        {
            var table = new DataTable();
            table.Columns.Add("LBSTRESC");
            table.Columns.Add("LBDRVFL");

            table.Rows.Add("", "Y"); // Should fail
            table.Rows.Add("Some Value", "Y"); // Should pass
            table.Rows.Add(DBNull.Value, "Y"); // Should fail

            var result = _rule.Validate(table);

            Assert.Equal(new List<int> { 0, 2 }, result); // Rows 0 and 2 should fail
        }

        [Fact]
        public void Validate_IgnoresRows_WhenLBDRVFLIsNotY()
        {
            var table = new DataTable();
            table.Columns.Add("LBSTRESC");
            table.Columns.Add("LBDRVFL");

            table.Rows.Add("", "");
            table.Rows.Add("", "");

            var result = _rule.Validate(table);

            Assert.Empty(result); // No rows should fail because LBDRVFL is empty
        }
    }
}
