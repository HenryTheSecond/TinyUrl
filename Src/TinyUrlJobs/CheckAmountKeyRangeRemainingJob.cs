using Microsoft.EntityFrameworkCore;
using Quartz;
using Shared.Models.Database;
using TinyUrlJobs.Interfaces.Repositories.EntityFramework;

namespace TinyUrlJobs
{
    [DisallowConcurrentExecution]
    public class CheckAmountKeyRangeRemainingJob(IUrlRangeRepository urlRangeRepository) : IJob
    {
        private const int Threshold = 1000;
        private const int BatchSize = 1000;
        private static readonly Random random = new();

        public async Task Execute(IJobExecutionContext context)
        {
            long countRemaining = await urlRangeRepository.CountRemaining();

            if (countRemaining > Threshold)
                return;

            int numberOfCharacter = (await urlRangeRepository.Query
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync())?.Url.Length ?? 0;
            numberOfCharacter++;
            List<string> urls = [];
            char[] cur = new char[numberOfCharacter];

            await GenerateNewUrls(0);

            // Insert remaining urls if total % BatchSize > 0
            if(urls.Count > 0)
            {
                await InsertNewUrls(urls);
            }

            async Task GenerateNewUrls(int index)
            {
                if(index == numberOfCharacter)
                {
                    urls.Add(new string(cur));

                    if(urls.Count == BatchSize)
                    {
                        await InsertNewUrls(urls);
                        urls.Clear();
                    }
                    return;
                }

                List<char> chars = GeneratePermutation(UrlRangeConfiguration.UrlAcceptedCharacters);
                foreach(var ch in chars)
                {
                    cur[index] = ch;
                    await GenerateNewUrls(index + 1);
                }
            }
        }
    
        private async Task InsertNewUrls(List<string> urls)
        {
            foreach (var url in urls)
            {
                urlRangeRepository.Add(new UrlRange
                {
                    Id = Ulid.NewUlid(),
                    Url = url,
                    IsUsed = false
                });
            }
            await urlRangeRepository.SaveChangeAsync();
        }

        private List<char> GeneratePermutation(string str)
        {
            var chars = str.ToList();
            for (int i = str.Length - 1; i >= 1; i--)
            {
                var randomIndex = random.Next(0, i);

                var tmp = chars[randomIndex];
                chars[randomIndex] = chars[i];
                chars[i] = tmp;
            }

            return chars;
        }
    }
}
