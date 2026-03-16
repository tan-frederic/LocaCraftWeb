using System.ComponentModel.DataAnnotations;

namespace LocaCraftAPI.DTOs.Lease
{
    public class LeaseCreateDTO
    {
        public int RealEstateAssetId { get; set; }

        public int LessorId { get; set; }

        public string LeaseName { get; set; } = string.Empty;

        [Range(0, double.MaxValue)]
        public decimal MonthlyRent { get; set; }

        [Range(0, double.MaxValue)]
        public decimal MonthlyCharges { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Deposit { get; set; }

        public double? RentIndexReference { get; set; }

        public bool IsOngoing { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
