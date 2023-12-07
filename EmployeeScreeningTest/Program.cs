using Microsoft.Azure.Cosmos;
using EmployeeScreeningTest.Services.Interfaces;
using EmployeeScreeningTest.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Register the Cosmos DB client and EmployeeRepository service
builder.Services.AddSingleton<CosmosClient>(sp => new CosmosClient(builder.Configuration["CosmosDb:ConnectionString"]));
builder.Services.AddSingleton<IEmployeeRepository, EmployeeRepository>(sp =>
{
    var cosmosClient = sp.GetRequiredService<CosmosClient>();
    string databaseName = builder.Configuration["CosmosDb:DatabaseName"];
    string containerName = builder.Configuration["CosmosDb:ContainerName"];
    return new EmployeeRepository(cosmosClient, databaseName, containerName);
});

// Register the EmployeeService service
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

// TODO: Register Redis Cache client and related services when ready

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
