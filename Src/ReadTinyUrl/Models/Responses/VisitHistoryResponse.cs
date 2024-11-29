namespace ReadTinyUrl.Models.Responses
{
    public class VisitHistoryResponse
    {
        public string Id { get; set; } = string.Empty;
        public string TinyUrl { get; set; } = string.Empty;
        public string OriginalUrl { get; set; } = string.Empty;
        public DateTimeOffset VisitedTime { get; set; }
    }
}
