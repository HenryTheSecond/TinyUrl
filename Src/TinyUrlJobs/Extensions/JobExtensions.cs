using Quartz;
namespace TinyUrlJobs.Extensions;

public static class JobExtensions
{
    public static void ConfigureJob<TJob>(this IServiceCollectionQuartzConfigurator quartz, string identity, TimeSpan interval) where TJob : IJob
    {
        quartz.ScheduleJob<TJob>(triggerOptions => triggerOptions
            .WithIdentity(identity)
            .StartNow()
        .WithSimpleSchedule(x => x
                .WithInterval(interval)
                .RepeatForever()));
    }
}
