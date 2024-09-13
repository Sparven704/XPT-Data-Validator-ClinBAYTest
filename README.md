# XPT Data Validator

Welcome to the **XPT Data Validator**, a tool designed to validate XPT data files against specific validation rules and generate a detailed Excel report of the results.

## Features
- Upload one or more `.xpt` files.
- Validate the datasets based on custom rules.
- Generate an Excel report with the validation results.

## Requirements
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- [xUnit](https://xunit.net/) (for running unit tests)

## Installation

1. Clone the repository:
    ```bash
    git clone https://github.com/your-username/xpt-data-validator.git
    cd xpt-data-validator
    ```

2. Restore the required packages:
    ```bash
    dotnet restore
    ```

3. Build the application:
    ```bash
    dotnet build
    ```

## Running the Application

1. To start the application, use the following command:
    ```bash
    dotnet run
    ```

2. Open your browser and navigate to:
    ```
    https://localhost:7284
    ```

3. You will be presented with a simple interface where you can:
    - Upload one or more `.xpt` files.
    - Click "Validate" to run the validation.
    - Download the generated Excel report once the validation is complete.

## Validation Rules

### EXDURValidationRule
- This rule checks that the `EXDUR` column values are non-negative and not missing.
- Rows where the `EXDUR` value is less than 0 will fail the validation.

### LBSTRESCValidationRule
- This rule checks that the `LBSTRESC` column is not empty when the `LBDRVFL` column has a value of "Y".
- Rows where `LBDRVFL` is "Y" and `LBSTRESC` is empty will fail the validation.

## Running Unit Tests

1. To run the unit tests for the validation rules, use the following command:
    ```bash
    dotnet test
    ```

2. The tests will verify the functionality of each validation rule to ensure correctness.

## Example Excel Report

The Excel report will contain:
- A separate worksheet for each dataset uploaded, displaying the raw data.
- A combined validation results worksheet, summarizing any failed rows based on the rules.

