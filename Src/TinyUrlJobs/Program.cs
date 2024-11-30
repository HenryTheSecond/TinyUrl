using MongoDB.Driver;
using Quartz;
using TinyUrlJobs;
using Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using TinyUrlJobs.Extensions;
using TinyUrlJobs.Jobs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddQuartz(q =>
{
    var configuration = builder.Configuration;
    q.ConfigureJob<CleanExpiredUrlJob>("CleanExpireUrl", configuration.GetValue<TimeSpan>("CleanExpireUrlJobInterval"));
    q.ConfigureJob<CheckAmountKeyRangeRemainingJob>("CheckAmountKeyRangeRemaining", configuration.GetValue<TimeSpan>("CheckAmountKeyRangeRemainingJobInterval"));
    q.ConfigureJob<AggregateVisitedTimeJob>("AggregateVisitedTime", configuration.GetValue<TimeSpan>("AggregateVisitedTimeJobInterval"));
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

