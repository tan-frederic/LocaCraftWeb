namespace LocaCraftAPI.DTOs.Lease
{
    public class LeaseSummaryDTO
    {
        public int Id { get; set; }
        public string LeaseName { get; set; } = string.Empty;
        public decimal MonthlyRent { get; set; }
        public decimal MonthlyCharges { get; set; }
        public decimal Deposit { get; set; }
        public bool IsOngoing { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
