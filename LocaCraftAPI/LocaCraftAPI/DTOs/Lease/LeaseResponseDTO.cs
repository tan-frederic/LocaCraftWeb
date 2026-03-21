namespace LocaCraftAPI.DTOs.Lease
{
    public class LeaseResponseDTO
    {
        public int Id { get; set; }

        public int RealEstateAssetId { get; set; }
        public string RealEstateAssetName { get; set; } = string.Empty;

        public int LessorId { get; set; }
        public string LessorFirstName { get; set; } = string.Empty;
        public string LessorLastName { get; set; } = string.Empty;

        public string LeaseName { get; set; } = string.Empty;
        public decimal MonthlyRent { get; set; }
        public decimal MonthlyCharges { get; set; }
        public decimal Deposit { get; set; }

        public double? RentIndexReference { get; set; }
        public bool IsOngoing { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public List<TenantSummaryDto> Tenants { get; set; } = new();
    }

    public class TenantSummaryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
