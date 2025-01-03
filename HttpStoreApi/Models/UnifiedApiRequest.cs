namespace HttpStoreApi.Models
{
    public class UnifiedApiRequest
    {
        public string ApiId { get; set; } = string.Empty;
        public object Request { get; set; }
    }
}
