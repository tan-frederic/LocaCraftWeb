using LocaCraftAPI.DTOs.Lease;
using System.ComponentModel.DataAnnotations;

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
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public List<LeaseSummaryDTO> Leases { get; set; } = new();
    }
}
