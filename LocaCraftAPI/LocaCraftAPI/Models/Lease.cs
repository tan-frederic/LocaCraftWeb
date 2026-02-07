namespace LocaCraftAPI.Models
{
    public class Lease
    {
        public int Id { get; set; }
        public int RealEstateAssetId { get; set; }
        public RealEstateAsset? RealEstateAsset { get; set; }
        public string LeaseeName { get; set; } = string.Empty;
        public decimal MonthlyRent { get; set; }
        public decimal MonthlyCharges { get; set; }
        public decimal Deposit { get; set; }

        public List<Tenant> Tenants { get; set; } = new List<Tenant>();
        public List<LeaseDocuments> LeaseDocuments { get; set; } = new List<LeaseDocuments>();

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
