namespace LocaCraftAPI.Models
{
    public class LeaseDocuments
    {
        public int Id { get; set; }
        public string DocumentName { get; set; } = string.Empty;
        public string DocumentType { get; set; } = string.Empty;
        public byte[] DocumentContent { get; set; } = Array.Empty<byte>();
        public int LeaseId { get; set; }
        public Lease? Lease { get; set; }
    }
}
