namespace ReadTinyUrl.Interfaces.Services
{
    public interface ITinyUrlService
    {
        Task<string> ReadUrlAsync(string tinyUrl);
    }
}
