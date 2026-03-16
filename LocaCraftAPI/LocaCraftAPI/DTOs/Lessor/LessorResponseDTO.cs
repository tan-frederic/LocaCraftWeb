namespace LocaCraftAPI.DTOs.Lessor
{
    public class LessorResponseDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<LeaseSummaryDto> Leases { get; set; } = new();
    }

    public class LeaseSummaryDto
    {
        public int Id { get; set; }
        public string LeaseName { get; set; } = string.Empty;
        public decimal MonthlyRent { get; set; }
        public decimal MonthlyCharges { get; set; }
        public decimal Deposit { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
