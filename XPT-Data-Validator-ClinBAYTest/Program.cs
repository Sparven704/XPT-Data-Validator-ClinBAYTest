using System.Data;
using Microsoft.OpenApi.Models;
using XPT_Data_Validator_ClinBAYTest.Interfaces;
using XPT_Data_Validator_ClinBAYTest.Services;

namespace XPT_Data_Validator_ClinBAYTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddScoped<IDataValidatorService, DataValidatorService>();
            builder.Services.AddScoped<IReportGeneratorService, ReportGeneratorService>();
            builder.Services.AddScoped<IXptReaderService, XptReaderService>();

            // Add services to the container.
            builder.Services.AddRazorPages();

            // Add Swagger services
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "XPT Data Validator API",
                    Description = "An API to validate XPT data files and generate reports"
                });

                
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "XPT Data Validator API v1");
            });

            app.MapRazorPages();

            // Validate the uploaded XPT file and generate a report
            app.MapPost("/validate", async (IFormFileCollection files, IXptReaderService xptReaderService, IDataValidatorService dataValidatorService, IReportGeneratorService reportGeneratorService) =>
            {
                if (files == null || files.Count == 0)
                {
                    return Results.BadRequest("No files uploaded.");
                }

                var datasets = new List<DataTable>();

                foreach (var file in files)
                {
                    var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
                    if (fileExtension != ".xpt")
                    {
                        return Results.BadRequest("Invalid file type. Only .xpt files are allowed.");
                    }

                    // Load the dataset from each uploaded file
                    await using var stream = new MemoryStream();
                    await file.CopyToAsync(stream);
                    stream.Position = 0;
                    datasets.Add(await xptReaderService.LoadDatasetAsync(stream, Path.GetFileNameWithoutExtension(file.FileName)));
                }

                // Validate all datasets
                var validationResults = dataValidatorService.ValidateDatasets(datasets);

                // Generate the report
                await using var reportStream = new MemoryStream();
                reportGeneratorService.GenerateReport(validationResults, datasets, reportStream);

                // Copy the reportStream to a new stream to prevent the ObjectDisposedException
                var fileStream = new MemoryStream(reportStream.ToArray());

                // Return the report
                fileStream.Position = 0;
                return Results.File(fileStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ValidationReport.xlsx");
            }).DisableAntiforgery();

            app.Run();
        }
    }
}