namespace ECommerceView.Models
{
    public class SyncCartResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public long? CartId { get; set; }
    }
}
