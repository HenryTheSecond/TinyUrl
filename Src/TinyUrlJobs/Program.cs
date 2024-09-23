using MongoDB.Driver;
using Quartz;
using TinyUrlJobs;
using TinyUrlJobs.Interfaces.Repositories;
using TinyUrlJobs.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddQuartz(q =>
{
    q.ScheduleJob<CleanExpiredUrlJob>(triggerOptions => triggerOptions
        .WithIdentity("CleanExpireUrl")
        .StartNow()
        .WithSimpleSchedule(x => x
            .WithInterval(builder.Configuration.GetValue<TimeSpan>("CleanExpireUrlJobInterval"))
            .RepeatForever()));
});
builder.Services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

builder.Services.AddSingleton(services => new MongoClient(builder.Configuration.GetConnectionString("TinyUrl")));
builder.Services.AddSingleton<ITinyUrlRepository, TinyUrlRepository>();

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

