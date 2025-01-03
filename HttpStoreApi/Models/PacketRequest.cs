namespace HttpStoreApi.Models
{
    public class PacketRequest
    {
        public string PacketId { get; set; }
        public string SagaId { get; set; }
        public PacketDetails Request { get; set; }
    }

    public class PacketDetails
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string MaId { get; set; }
        public string DateTime { get; set; }
        public PayloadDetails Payload { get; set; }
    }

    public class PayloadDetails
    {
        public string DateTime { get; set; }
        public string TargetTimeZone { get; set; }
    }
}
