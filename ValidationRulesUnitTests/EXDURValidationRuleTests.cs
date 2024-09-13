using System.Data;
using XPT_Data_Validator_ClinBAYTest.ValidationRules;

namespace ValidationRulesUnitTests
{
    public class EXDURValidationRuleTests
    {
        private readonly EXDURValidationRule _rule;

        public EXDURValidationRuleTests()
        {
            _rule = new EXDURValidationRule();
        }

        [Fact]
        public void IsApplicable_ReturnsFalse_WhenColumnIsMissing()
        {
            var table = new DataTable();
            table.Columns.Add("SomeOtherColumn");

            var result = _rule.IsApplicable(table);

            Assert.False(result);
        }

        [Fact]
        public void IsApplicable_ReturnsTrue_WhenColumnExists()
        {
            var table = new DataTable();
            table.Columns.Add("EXDUR");

            var result = _rule.IsApplicable(table);

            Assert.True(result);
        }

        [Fact]
        public void Validate_ReturnsEmpty_WhenNoNegativeValues()
        {
            var table = new DataTable();
            table.Columns.Add("EXDUR");

            table.Rows.Add(1);
            table.Rows.Add(0);
            table.Rows.Add(100);

            var result = _rule.Validate(table);

            Assert.Empty(result);
        }

        [Fact]
        public void Validate_ReturnsFailedRows_WhenNegativeValuesExist()
        {
            var table = new DataTable();
            table.Columns.Add("EXDUR");

            table.Rows.Add(-1); // Should fail
            table.Rows.Add(0);
            table.Rows.Add(-10); // Should fail

            var result = _rule.Validate(table);

            Assert.Equal(new List<int> { 0, 2 }, result); // Row 0 and 2 should fail
        }

        [Fact]
        public void Validate_IgnoresNullValues()
        {
            var table = new DataTable();
            table.Columns.Add("EXDUR");

            table.Rows.Add(DBNull.Value); // Should be ignored
            table.Rows.Add(1);
            table.Rows.Add(-5); // Should fail

            var result = _rule.Validate(table);

            Assert.Single(result);
            Assert.Equal(2, result[0]); // Only row 2 should fail
        }
    }
}