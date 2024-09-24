using Quartz;
using TinyUrlJobs.Interfaces.Repositories.EntityFramework;

namespace TinyUrlJobs
{
    public class CheckAmountKeyRangeRemainingJob(IUrlRangeRepository urlRangeRepository) : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            long a = await urlRangeRepository.CountRemaining();
        }
    }
}
