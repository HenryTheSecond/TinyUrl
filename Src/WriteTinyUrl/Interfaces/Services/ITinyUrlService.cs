namespace WriteTinyUrl.Interfaces.Services
{
    public interface ITinyUrlService
    {
        Task<string> CreateTinyUrlAsync(string originalUrl);
    }
}
