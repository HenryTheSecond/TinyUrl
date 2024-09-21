using MongoDB.Driver;
using Shared.Extensions;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddExportedServices();

builder.Services.AddSingleton(service => new MongoClient(builder.Configuration.GetConnectionString("TinyUrl")));

builder.Services.AddSingleton(service =>
{
    var options = new ConfigurationOptions
    {
        EndPoints =
        {
            { 
                builder.Configuration.GetConnectionString("Redis:Host")!, 
                builder.Configuration.GetValue<int>("ConnectionStrings:Redis:Port")
            }
        }
    };
    var cluster = ConnectionMultiplexer.Connect(options);
    return cluster.GetDatabase();
});

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
