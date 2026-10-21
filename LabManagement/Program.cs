var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Configure SQL connection factory and DI for repository and service
builder.Services.AddSingleton<LabManagement.Data.ISqlConnectionFactory, LabManagement.Data.SqlConnectionFactory>();
builder.Services.AddScoped<LabManagement.Data.LabDbContext>();
builder.Services.AddScoped<LabManagement.Interfaces.IPatientRepository, LabManagement.Repositories.PatientRepository>();
builder.Services.AddScoped<LabManagement.Interfaces.IPatientService, LabManagement.Services.PatientService>();
builder.Services.AddScoped<LabManagement.Interfaces.IOrderRepository, LabManagement.Repositories.OrderRepository>();
builder.Services.AddScoped<LabManagement.Interfaces.IOrderService, LabManagement.Services.OrderService>();
builder.Services.AddScoped<LabManagement.Interfaces.ITestMasterRepository, LabManagement.Repositories.TestMasterRepository>();
builder.Services.AddScoped<LabManagement.Interfaces.ITestMasterService, LabManagement.Services.TestMasterService>();
builder.Services.AddScoped<LabManagement.Interfaces.IOrganizationRepository, LabManagement.Repositories.OrganizationRepository>();
builder.Services.AddScoped<LabManagement.Interfaces.IOrganizationService, LabManagement.Services.OrganizationService>();
builder.Services.AddScoped<LabManagement.Interfaces.ILegalEntityRepository, LabManagement.Repositories.LegalEntityRepository>();
builder.Services.AddScoped<LabManagement.Interfaces.ILegalEntityService, LabManagement.Services.LegalEntityService>();
builder.Services.AddScoped<LabManagement.Interfaces.IAuthRepository, LabManagement.Repositories.AuthRepository>();
builder.Services.AddScoped<LabManagement.Interfaces.IAuthService, LabManagement.Services.AuthService>();
builder.Services.AddScoped<LabManagement.Interfaces.IResultRepository, LabManagement.Repositories.ResultRepository>();
builder.Services.AddScoped<LabManagement.Interfaces.IResultService, LabManagement.Services.ResultService>();
builder.Services.AddScoped<LabManagement.Interfaces.IRangeValidationService, LabManagement.Services.RangeValidationService>();
builder.Services.AddScoped<LabManagement.Interfaces.IFormulaCalculator, LabManagement.Services.LdlCalculator>();
builder.Services.AddScoped<LabManagement.Interfaces.IFormulaCalculator, LabManagement.Services.BmiCalculator>();
builder.Services.AddScoped<LabManagement.Interfaces.IFormulaCalculator, LabManagement.Services.EgfrCalculator>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngular");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
