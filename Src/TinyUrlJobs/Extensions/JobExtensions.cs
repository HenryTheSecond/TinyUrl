using Quartz;

namespace TinyUrlJobs.Extensions
{
    public static class JobExtensions
    {
        public static void ConfigureCleanExpiredUrlJob(this IServiceCollectionQuartzConfigurator quartz, ConfigurationManager configuration)
        {
            quartz.ScheduleJob<CleanExpiredUrlJob>(triggerOptions => triggerOptions
                .WithIdentity("CleanExpireUrl")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithInterval(configuration.GetValue<TimeSpan>("CleanExpireUrlJobInterval"))
                    .RepeatForever()));
        }

        public static void ConfigureCheckAmountKeyRangeRemainingJob(this IServiceCollectionQuartzConfigurator quartz, ConfigurationManager configuration)
        {
            quartz.ScheduleJob<CheckAmountKeyRangeRemainingJob>(triggerOptions => triggerOptions
                .WithIdentity("CheckAmountKeyRangeRemaining")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithInterval(configuration.GetValue<TimeSpan>("CheckAmountKeyRangeRemainingJobInterval"))
                    .RepeatForever()));
        }
    }
}
