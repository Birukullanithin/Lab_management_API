var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Configure SQL connection factory and DI for repository and service
builder.Services.AddSingleton<LabManagement.Data.ISqlConnectionFactory, LabManagement.Data.SqlConnectionFactory>();
builder.Services.AddScoped<LabManagement.Data.LabDbContext>();
builder.Services.AddScoped<LabManagement.Interfaces.IPatientRepository, LabManagement.Repositories.PatientRepository>();
builder.Services.AddScoped<LabManagement.Interfaces.IPatientService, LabManagement.Services.PatientService>();
builder.Services.AddScoped<LabManagement.Interfaces.IOrganizationRepository, LabManagement.Repositories.OrganizationRepository>();
builder.Services.AddScoped<LabManagement.Interfaces.IOrganizationService, LabManagement.Services.OrganizationService>();
builder.Services.AddScoped<LabManagement.Interfaces.ILegalEntityRepository, LabManagement.Repositories.LegalEntityRepository>();
builder.Services.AddScoped<LabManagement.Interfaces.ILegalEntityService, LabManagement.Services.LegalEntityService>();
builder.Services.AddScoped<LabManagement.Interfaces.IAuthRepository, LabManagement.Repositories.AuthRepository>();
builder.Services.AddScoped<LabManagement.Interfaces.IAuthService, LabManagement.Services.AuthService>();
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
