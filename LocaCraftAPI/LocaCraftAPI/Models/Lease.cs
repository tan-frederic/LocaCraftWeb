using System.ComponentModel.DataAnnotations;

namespace LocaCraftAPI.Models
{
    public class Lease
    {
        public int Id { get; set; }

        public int RealEstateAssetId { get; set; }
        public RealEstateAsset? RealEstateAsset { get; set; }

        public int LessorId { get; set; }
        public Lessor? Lessor { get; set; }

        public string LeaseName { get; set; } = string.Empty;
        public decimal MonthlyRent { get; set; }
        public decimal MonthlyCharges { get; set; }
        public decimal Deposit { get; set; }
        public double? RentIndexReference { get; set; }
        public bool IsOngoing { get; set; }

        public List<Tenant> Tenants { get; set; } = new List<Tenant>();
        public List<LeaseDocuments> LeaseDocuments { get; set; } = new List<LeaseDocuments>();

        [Required]
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

    }
}
