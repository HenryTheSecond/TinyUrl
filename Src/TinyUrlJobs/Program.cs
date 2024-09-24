using MongoDB.Driver;
using Quartz;
using TinyUrlJobs;
using Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using TinyUrlJobs.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddQuartz(q =>
{
    q.ConfigureCleanExpiredUrlJob(builder.Configuration);
    q.ConfigureCheckAmountKeyRangeRemainingJob(builder.Configuration);
});

builder.Services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

builder.Services.AddSingleton(services => new MongoClient(builder.Configuration.GetConnectionString("TinyUrl")));

builder.Services.AddDbContext<UrlRangeContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("UrlRangeContext")!));
builder.Services.AddTransactionContext<UrlRangeContext>();

builder.Services.AddExportedServices();

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

