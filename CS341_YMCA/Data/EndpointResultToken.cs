namespace CS341_YMCA.Data
{
    public class EndpointResultToken
    {
        public bool Success { get; set; } = true;
        public string? Error { get; set; } = null;
        public Guid TransactionGuid { get; set; } = Guid.NewGuid();
    }
}
