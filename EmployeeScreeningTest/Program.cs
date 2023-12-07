using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.Cosmos;
using Azure.Identity;
using EmployeeScreeningTest.Services.Interfaces;
using EmployeeScreeningTest.Services.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Caching.Distributed;

var builder = WebApplication.CreateBuilder(args);

// Add configuration sources (appsettings.json, appsettings.Development.json, etc.).
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Add Azure Key Vault using the new method
var keyVaultUrl = new Uri($"https://{builder.Configuration["KeyVaultName"]}.vault.azure.net/");
builder.Configuration.AddAzureKeyVault(keyVaultUrl, new DefaultAzureCredential());

// Add services to the container.
builder.Services.AddControllers();

// Register the Redis Cache client
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["RedisCacheConnectionString"];
});

// Register the Cosmos DB client and EmployeeRepository service
builder.Services.AddSingleton<CosmosClient>(sp =>
{
    var connectionString = builder.Configuration["CosmosDbConnectionString"];
    return new CosmosClient(connectionString);
});
builder.Services.AddSingleton<IEmployeeRepository, EmployeeRepository>(sp =>
{
    var cosmosClient = sp.GetRequiredService<CosmosClient>();
    var cache = sp.GetRequiredService<IDistributedCache>();
    string databaseName = builder.Configuration["CosmosDatabaseName"];
    string containerName = builder.Configuration["CosmosContainerName"];
    return new EmployeeRepository(cosmosClient, databaseName, containerName, cache);
});


// Register the EmployeeService service
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

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
